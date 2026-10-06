# docs.functions

Azure Functions that publish GitHub-hosted blog content for [red-folder.com](https://red-folder.com). The service converts Markdown to HTML, copies images to Azure Blob Storage, maintains blog metadata, and emails an audit report after processing.

## Processing flow

1. `GithubWebhook` accepts a GitHub push webhook, stores its commits in a staging blob, and queues the request ID.
2. `Loader` consumes the request ID, reads the stored commits, and retrieves source files from GitHub at the relevant commit.
3. Added or modified blogs are transformed and uploaded; deleted blogs and images are removed. Metadata is stored as `BlogMeta.json` in Blob Storage.
4. The request blob is deleted after processing, and the SendGrid output binding sends an HTML audit report.
5. `Blog` provides an HTTP API for reading the metadata.

Each source blog folder contains `blog.md`, `blog.json`, and any images. Supported image extensions are `.png`, `.jpg`, and `.gif`. Generated HTML is stored as `<blog-url>/<blog-url>.html` (lowercase). See the [test assets](tests/DocFunctions.Integration/Assets) for example content and metadata.

## Functions

Routes below use the default `/api` prefix. Bindings are defined in `src/DocFunctions/<function>/function.json`.

| Function | Trigger / route | Behavior |
| --- | --- | --- |
| `GithubWebhook` | HTTP, `/api/GithubWebhook`, legacy GitHub webhook binding | Queues commits for asynchronous processing. |
| `Loader` | Queue `to-be-processed`, connection `BlobStorage` | Publishes content and metadata; sends an audit email. |
| `Blog` | GET `/api/Blog/{blogUrl=*}`, function authorization | Returns one blog, or all metadata when `blogUrl` is the literal `*`; a missing individual blog returns 404. |
| `ResyncAll` | HTTP, `/api/ResyncAll` | Clears the configured content container, then queues a full repository sync. |

**ResyncAll deletes existing content before rebuilding it.** When content and metadata share the `blog` container, this also deletes `BlogMeta.json`. Use only when a rebuild is intended; this is not a health-check endpoint. Its repository scan covers files in immediate child folders of the repository root.

## Associated Azure resources

Verified through read-only Azure CLI queries on **6 October 2026**. Both Function Apps reported `Running`, and Azure listed `Blog`, `GithubWebhook`, `Loader`, and `ResyncAll` in each app. This verifies resource/configuration presence, not successful end-to-end processing or deployed binary parity with this checkout.

| Resource / setting | Production | Staging |
| --- | --- | --- |
| Subscription | Red Folder Production | Red Folder Staging |
| Subscription ID | `20366cfc-092d-46d6-a630-a687d3a04992` | `276e2abc-6a34-4925-b24f-73913402ea4a` |
| Function App and resource group | `rfc-docs-production` | `rfc-doc-functions-staging` |
| Function hostname | `rfc-docs-production.azurewebsites.net` | `rfc-doc-functions-staging.azurewebsites.net` |
| App Service plan | `NorthEuropePlan` in `rfc-docs-production` | `NorthEuropePlan` in `rfc-doc-functions-staging` |
| Plan SKU / tier | `Y1` / `Dynamic` | `Y1` / `Dynamic` |
| Content, metadata, and work storage account | `rfcdocsproduction` in `rfc-docs-production` | `rfcdocs` in `rfc-docs` |
| Functions host and file-content storage account | `rfcdocsproduction` | `function861a0142832f` in `rfc-doc-functions-staging` |
| Application Insights component | `rfc-docs-production` in `rfc-docs-production` | `rfc-doc-functions-staging` in `rfc-doc-functions-staging` |
| Failure anomaly alert | `Failure Anomalies - rfc-docs-production` | `Failure Anomalies - rfc-doc-functions-staging` |
| Source GitHub repository | `Red-Folder/red-folder.docs.production` | `Red-Folder/red-folder.docs.staging` |
| `ContentBaseUrl` | `https://rfcdocsproduction.blob.core.windows.net/blog` | `https://rfcdocs.blob.core.windows.net/blog` |
| `MediaBaseUrl` | `https://content.red-folder.com/blog` | `https://rfcdocs.blob.core.windows.net/blog` |
| Configured Functions runtime | `~1` | `~1` |

The Function Apps, plans, storage accounts, and Application Insights components above are in North Europe. Failure anomaly alerts are global resources. Production Application Insights also has the managed workspace `managed-rfc-docs-production-ws` in resource group `ai_rfc-docs-production_7e060ad5-ddde-43bf-af2c-7a57a4057c47_managed`.

The staging subscription also contains the SaaS resource `redfolder` in resource group `sendgrid`. The code and deployed settings confirm SendGrid usage, but do not establish which SendGrid account the configured API keys belong to. The production media URL is configured as shown; its DNS/CDN origin was not verified.

### Storage mapping

The deployed custom connection strings `BlobStorage`, `BlogMetaStorage`, and `ToBeProcessedStorage` all reference the content/work storage account in the table above for their respective environment.

| Purpose | Configuration | Configured name in both environments |
| --- | --- | --- |
| Generated HTML and images | `BlobStorage` + `BlobStorageContainerName` | Blob container `blog` |
| Metadata (`BlogMeta.json`) | `BlogMetaStorage` + `BlogMetaStorageContainerName` | Blob container `blog` |
| Pending commit payloads | `ToBeProcessedStorage` + `ToBeProcessedContainerName` | Blob container `to-be-processed` |
| Work request IDs | `ToBeProcessedStorage` + `ToBeProcessedQueueName` | Queue `to-be-processed` |

These names were verified from application settings; individual containers, blobs, and queues were not enumerated. Storage clients create the configured containers and queue if they do not exist.

**Queue configuration must agree:** the producer uses `ToBeProcessedStorage` and `ToBeProcessedQueueName`, but the loader binding hardcodes queue `to-be-processed` on connection `BlobStorage`. Both connections must target the same queue account, and the configured queue name must match the binding. The observed deployment settings satisfy this mapping.

## Configuration

Application code reads `ConfigurationManager.AppSettings` and `ConfigurationManager.ConnectionStrings`. Configure these in the Function App or the appropriate local `.config` file. The checked-in `Web.config` contains framework/binding configuration, but no application credentials or storage settings.

| Application setting | Purpose |
| --- | --- |
| `github-username` | GitHub repository owner and authentication username. |
| `github-key` | GitHub credential used to read source content. |
| `github-repo` | Source repository name. |
| `BlobStorageContainerName` | Destination container for HTML and images. |
| `BlogMetaStorageContainerName` | Container holding `BlogMeta.json`. |
| `ToBeProcessedContainerName` | Container holding pending commit payloads. |
| `ToBeProcessedQueueName` | Queue receiving request IDs; must be `to-be-processed` with the current binding. |
| `ContentBaseUrl` | Base URL used to generate blog HTML links. |
| `MediaBaseUrl` | Base URL used to rewrite media links. |
| `EmailFrom`, `EmailTo` | Audit email sender and recipient; required by `Loader`. |
| `AzureWebJobsSendGridApiKey` | API key for the SendGrid output binding. |
| `APPINSIGHTS_INSTRUMENTATIONKEY` | Optional application telemetry setting read by `Blog`. |
| `AzureWebJobsStorage` | Functions host storage. |
| `AzureWebJobsDashboard` | Legacy dashboard storage setting present in both deployments. |
| `FUNCTIONS_EXTENSION_VERSION` | Runtime selection; both deployments currently specify `~1`. |

Required named connection strings: `BlobStorage`, `BlogMetaStorage`, and `ToBeProcessedStorage` (deployed as type `Custom`). The loader binding also resolves `BlobStorage`.

Both apps have `WEBSITE_CONTENTAZUREFILECONNECTIONSTRING`, `WEBSITE_CONTENTSHARE`, and `WEBSITE_RUN_FROM_PACKAGE` settings. These are deployment/host configuration; their values and package sources are not documented here. Keep storage keys, GitHub credentials, SendGrid keys, function keys, and publishing credentials outside source control.

## Repository layout

| Path | Responsibility |
| --- | --- |
| `src/DocFunctions` | Function entry points, bindings, and client configuration. |
| `src/DocFunctions.Lib` | GitHub/storage clients, content actions, queue processing, and audit reports. |
| `src/DocFunctions.Markdown` | Markdown conversion and custom content transformations. |
| `src/docsFunctions.Shared` | Blog and redirect models. |
| `tests/DocFunctions.Lib.Unit` | Unit tests for actions, builders, and metadata processing. |
| `tests/DocFunctions.Markdown.Unit` | Markdown transformer unit tests. |
| `tests/DocFunctions.Lib.Integration` | Integration tests using GitHub and Azure Storage. |
| `tests/DocFunctions.Integration` | SpecFlow scenarios with local fakes or external services. |

## Build and tests

This is a legacy, non-SDK-style C# solution using `packages.config`. The Function project targets .NET Framework 4.6; supporting projects also target 4.5.2. Use Windows with Visual Studio/MSBuild, the ASP.NET web application build targets, NuGet CLI, and the corresponding .NET Framework targeting packs.

From a Visual Studio Developer PowerShell with NuGet on `PATH`:

```powershell
nuget restore .\docs.functions.sln
msbuild .\docs.functions.sln /p:Configuration=Debug
```

Run `DocFunctions.Lib.Unit` and `DocFunctions.Markdown.Unit` through Visual Studio Test Explorer. The library unit tests can also be run with the restored console runner:

```powershell
.\packages\xunit.runner.console.2.3.1\tools\net452\xunit.console.x86.exe .\tests\DocFunctions.Lib.Unit\bin\Debug\DocFunctions.Lib.Unit.dll
```

After building, `UnitTestsAndCodeCoverage.ps1` runs **only the library unit suite** through OpenCover and generates coverage/Cobertura reports in `TestAndCoverage`. It clears that output directory on each run.

Integration tests require separate configuration and may write or delete GitHub/Azure content. Use disposable test resources. `DocFunctions.Integration` defaults to external clients when `UseLocalFake` is absent or invalid; set `UseLocalFake=true` in its test application settings to use local fakes. External scenarios use `github-username`, `github-key`, `github-repo`, and `AzureFunctionKey`; their staging Function App and blob URLs are hardcoded in `tests/DocFunctions.Integration/Models/Config.cs`. Library integration tests read the named storage connection strings and application settings directly from test configuration. Debug transforms for both integration projects are ignored by Git.

## Deployment and operations

The repository contains function bindings and compiled assembly references (`..\bin\DocFunctions.dll`), but no checked-in infrastructure templates, CI/CD pipeline, or publish profile. Building the solution alone does not provision Azure resources. Recover and verify the existing publishing process before deploying; do not assume current Functions tooling can run this legacy application unchanged.

For a read-only inventory refresh, use an authenticated Azure CLI session and an explicit subscription:

```powershell
az resource list --subscription 20366cfc-092d-46d6-a630-a687d3a04992 --resource-group rfc-docs-production --output table
az resource list --subscription 276e2abc-6a34-4925-b24f-73913402ea4a --resource-group rfc-doc-functions-staging --output table
az resource list --subscription 276e2abc-6a34-4925-b24f-73913402ea4a --resource-group rfc-docs --output table
az functionapp function list --subscription 20366cfc-092d-46d6-a630-a687d3a04992 --resource-group rfc-docs-production --name rfc-docs-production --query "[].name" --output table
```

When diagnosing processing, inspect function traces and the email audit. Some individual content actions catch and log errors instead of propagating them, so a completed queue request or audit email does not by itself prove every file published successfully. Cache client implementations exist, but `ClientFactory` currently constructs `AllCachesClient(null)`, so this processing path does not invalidate caches.

## License

See [LICENSE](LICENSE).
