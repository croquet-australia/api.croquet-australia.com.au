Feature: Emails to 2016 GC Open Doubles Entries

Scenario: Entry when paying for yourself only by cheque
    Given tournament slug is '2016/gc/open-doubles'
    And payingForPartner is 'false'
    And paymentMethod is 'cheque'
    When the entry is submitted
    Then an email using 'Doubles Event Only - Paying for yourself only - Player.txt' template is sent to the player
    And an email using 'Doubles Event Only - Paying for yourself only - Partner.txt' template is sent to the partner

Scenario: Entry when paying for yourself and your partner by cheque
    Given tournament slug is '2016/gc/open-doubles'
    And payingForPartner is 'true'
    And paymentMethod is 'cheque'
    When the entry is submitted
    Then an email using 'Doubles Event Only - Paying for yourself and your partner - Player.txt' template is sent to the player
    And an email using 'Doubles Event Only - Paying for yourself and your partner - Partner.txt' template is sent to the partner

Scenario: Entry when paying for yourself only by EFT
    Given tournament slug is '2016/gc/open-doubles'
    And payingForPartner is 'false'
    And paymentMethod is 'EFT'
    When the entry is submitted
    Then an email using 'Doubles Event Only - Paying for yourself only - Player.txt' template is sent to the player
    And an email using 'Doubles Event Only - Paying for yourself only - Partner.txt' template is sent to the partner

Scenario: Entry when paying for yourself and your partner by EFT
    Given tournament slug is '2016/gc/open-doubles'
    And payingForPartner is 'true'
    And paymentMethod is 'EFT'
    When the entry is submitted
    Then an email using 'Doubles Event Only - Paying for yourself and your partner - Player.txt' template is sent to the player
    And an email using 'Doubles Event Only - Paying for yourself and your partner - Partner.txt' template is sent to the partner
