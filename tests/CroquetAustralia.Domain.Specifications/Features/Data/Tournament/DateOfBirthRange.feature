Feature: DateOfBirthRange

Scenario: Is under 21 tournament
	Given tournament starts '24 Sep 2016 Australia/Melbourne'
	And tournament finishes '26 Sep 2016 Australia/Melbourne'
	And tournament is U21
	When I get TournamentDateOfBirthRange
	Then the result should not be null
	And result.MinimumValue should be '1 Jan 1995'
	And result.MaximumValue should be '24 Sep 2016' 
	And result.Under18 should be '25 Sep 1998' 

Scenario: Is not under 21 tournament
	Given tournament starts '24 Sep 2016 Australia/Melbourne'
	And tournament finishes '26 Sep 2016 Australia/Melbourne'
	And tournament is not U21
	When I get TournamentDateOfBirthRange
	Then the result should be null
