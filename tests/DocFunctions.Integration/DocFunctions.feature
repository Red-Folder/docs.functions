Feature: DocFunctions
	In order to allow easy blog posting
	As a blog writer
	I want to be have any blog save pushed to my website and made available for my loving public

@ApplicationTest
Scenario: On new blog
	Given I don't already have a blog with name of the current date and time
	Then I would expect the blog to not be available via the Blog API
	When I publish a new blog to my Github repo
	Then I allow 60 seconds
	Then I would expect the blog to be available via the Blog API
	And I would expect the image to be available via the website
