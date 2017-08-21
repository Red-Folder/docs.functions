Feature: DocFunctions
	In order to allow easy blog posting
	As a blog writer
	I want to be have any blog save pushed to my website and made available for my loving public

Scenario: On new blog
	Given I don't already have a blog with name of the current date and time
	Then I would expect the blog to not be available via the Blog API
	When I publish a new blog to my Github repo
	Then I allow 60 seconds
	Then I would expect the blog to be available via the Blog API
	And I would expect the image to be available via the website

Scenario: On deleting a blog
	Given I publish a new blog to my Github repo
	Then I allow 60 seconds
	Then I would expect the blog to be available via the Blog API
	And I would expect the image to be available via the website
	When I delete that blog from my Github repo
	Then I allow 60 seconds
	Then I would expect the blog to not be available via the Blog API
	And I would expect the image to not be available via the website

Scenario: On amending an image on an existing blog
	Given I publish a new blog to my Github repo
	Then I allow 60 seconds
	Then I would expect the image to be available via the website
	When I update that image
	Then I allow 60 seconds
	Then I would expect the new image to be available via the website

Scenario: On amending text on an existing blog
	Given I publish a new blog to my Github repo
	Then I allow 60 seconds
	Then I would expect the blog to be available via the Blog API
	When I update that blog test
	Then I allow 60 seconds
	Then I would expect the new blog text to be available via the website
