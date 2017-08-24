﻿using DocFunctions.Lib.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using DocFunctions.Lib.Models.Github;
using DocFunctions.Lib.Actions;
using DocFunctions.Lib.Models.Audit;
using DocFunctions.Lib.Wappers;

namespace DocFunctions.Lib
{
    public class WebhookActionBuilder
    {
        private IActionBuilder _actionBuilder;
        private IEmailClient _emailClient;

        public WebhookActionBuilder(IActionBuilder actionBuilder, IEmailClient emailClient)
        {
            _actionBuilder = actionBuilder;
            _emailClient = emailClient;
        }

        public void Process(WebhookData data)
        {
            AuditTree.Instance.StartOperation("Processing data received from Github Webhook function");

            foreach (var commit in data.Commits)
            {
                AuditTree.Instance.StartOperation($"Processing actions for commit: {commit.Sha}");
                _actionBuilder.Clear();

                GetNewBlogs(commit).ForEach(x => _actionBuilder.NewBlog(x));
                GetModifiedBlogs(commit).ForEach(x => _actionBuilder.ModifyBlog(x));
                GetDeletedBlogs(commit).ForEach(x => _actionBuilder.DeleteBlog(x));

                GetNewImages(commit).ForEach(x => _actionBuilder.NewImage(x));
                GetModifiedImages(commit).ForEach(x => _actionBuilder.ModifyImage(x));
                GetDeletedImages(commit).ForEach(x => _actionBuilder.DeleteImage(x));

                var actions = _actionBuilder.Build();
                Execute(actions);
                AuditTree.Instance.EndOperation();
            }

            AuditTree.Instance.EndOperation();

            if (_emailClient != null)
            {
                var auditHtml = new AuditAsHtml(AuditTree.Instance).ToString();
                _emailClient.Send(auditHtml);
            }
        }

        private List<Added> GetNewBlogs(Commit commit)
        {
            return commit
                    .Added
                    .Where(x => x.IsBlogFile)
                    .GroupBy(x => x.Path)
                    .Select(x => x.First())
                    .ToList();
        }

        private List<Added> GetNewImages(Commit commit)
        {
            return commit
                    .Added
                    .Where(x => x.IsImageFile)
                    .ToList();
        }

        private List<Modified> GetModifiedBlogs(Commit commit)
        {
            return commit
                    .Modified
                    .Where(x => x.IsBlogFile)
                    .GroupBy(x => x.Path)
                    .Select(x => x.First())
                    .ToList();
        }

        private List<Modified> GetModifiedImages(Commit commit)
        {
            return commit
                    .Modified
                    .Where(x => x.IsImageFile)
                    .ToList();
        }

        private List<Removed> GetDeletedBlogs(Commit commit)
        {
            return commit
                    .Removed
                    .Where(x => x.IsBlogFile)
                    .GroupBy(x => x.Path)
                    .Select(x => x.First())
                    .ToList();
        }

        private List<Removed> GetDeletedImages(Commit commit)
        {
            return commit
                    .Removed
                    .Where(x => x.IsImageFile)
                    .ToList();
        }

        private void Execute(IAction[] actions)
        {
            if (actions != null)
            {
                foreach (var action in actions)
                {
                    action.Execute();
                }
            }
        }
    }
}
