Feature: GET /tournaments

# todo: Add all tournaments
@ignore @todo
Scenario Outline: happy path
	When get /tournaments
	Then result should contain starts <starts>
	And result should contain discipline <discipline>
	And result should contain slug <slug>

	Examples:
		| starts                          | discipline | slug           |
		| 18 Feb 2017 Australia/Melbourne | gc         | u21-worlds-eoi |
