Feature: DateOfBirthRange

Background: 
	Given tournament starts '24 Sep 2016 Australia/Melbourne'
	And tournament finishes '26 Sep 2016 Australia/Melbourne'
	And tournament practiceStarts '23 Sep 2016 Australia/Melbourne'

Scenario: Is under 21 tournament
	Given tournament is U21
	When I get DateOfBirthRange
	Then the result should not be null
	And result.Minimum should be '1 Jan 1995'
	And result.Maximum should be '24 Sep 2016' 

Scenario: Is not under 21 tournament
	Given tournament is not U21
	When I get DateOfBirthRange
	Then the result should be null
