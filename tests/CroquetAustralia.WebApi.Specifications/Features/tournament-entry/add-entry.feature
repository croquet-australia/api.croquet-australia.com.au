Feature: /tournament-entry/add-entry

Scenario: Happy path
    Given a valid 'SubmitEntry' command
    When the command is posted to '/tournament-entry/add-entry'
    Then the response status code should be 'NoContent'

@ignore @todo
Scenario: todo
	Given todo
