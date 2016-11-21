Feature: Emails to 2017 GC World Qualifier EOI

Scenario Outline: happy path
    Given tournament slug is '2017/gc/wcf-wc-championship-qualifying-tournament-eoi'
    And paymentMethod is 'null'
    When the entry is submitted
    Then an email using '<emailTemplate>' template is sent to the player

	Examples:
		| emailTemplate                   |
		| 2017 GC World Qualifier EOI.txt |
