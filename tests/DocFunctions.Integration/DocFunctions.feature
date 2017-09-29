Feature: DocFunctions
	In order to allow easy blog posting
	As a blog writer
	I want to be have any blog save pushed to my website and made available for my loving public

Scenario: On new blog
	Given I don't already have a blog with name of the current date and time
	Then I would expect the blog to not be available via the Blog API
	When I start a new commit
	And Add blog.md to the commit
	And Add blog.json to the commit
	And Add Image.png to the commit
	And Push the commit with message "On new blog scenario"
	Then I allow 60 seconds
	Then I would expect the blog to be available via the Blog API
	And I would expect the image to be available via the website

Scenario: On deleting a blog
	Given I start a new commit
	And Add blog.md to the commit
	And Add blog.json to the commit
	And Add Image.png to the commit
	And Push the commit with message "On deleting a blog scenario - create the target blog"
	Then I allow 60 seconds
	Then I would expect the blog to be available via the Blog API
	And I would expect the image to be available via the website
	When I start a new commit
	And Delete blog.md from the commit
	And Delete blog.json from the commit
	And Delete Image.png from the commit
	And Push the commit with message "On deleting a blog scenario - delete target blog"
	Then I allow 60 seconds
	Then I would expect the blog to not be available via the Blog API
	And I would expect the image to not be available via the website

Scenario: On amending an image on an existing blog
	Given I start a new commit
	And Add blog.md to the commit
	And Add blog.json to the commit
	And Add Image.png to the commit
	And Push the commit with message "On amending an image on an existing blog scenario - create the target blog"
	Then I allow 60 seconds
	Then I would expect the image to be available via the website
	When I start a new commit
	And Replace Image.png with Image2.png in the commit
	And Push the commit with message "On amending an image on an existing blog scenario - modified image in the target blog"
	Then I allow 60 seconds
	Then I would expect the new image to be available via the website

Scenario: On amending text on an existing blog
	Given I start a new commit
	And Add blog.md to the commit
	And Add blog.json to the commit
	And Add Image.png to the commit
	And Push the commit with message "On amending the text on an existing blog scenario - create the target blog"
	Then I allow 60 seconds
	Then I would expect the blog to be available via the Blog API
	When I start a new commit
	And Replace blog.md with blog2.md in the commit
	And Replace blog.json with blog2.json in the commit
	And Push the commit with message "On amending the meta of an existing blog scenario - modified description is in the meta"
	Then I allow 60 seconds
	Then I would expect the new blog meta to be available via the website
