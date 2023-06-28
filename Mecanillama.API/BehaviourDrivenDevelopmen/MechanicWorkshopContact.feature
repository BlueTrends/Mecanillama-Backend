Feature: Mechanic Workshop Contact
  Scenario: Customer Contacts Mechanic Workshop
    Given the customer is on the chosen mechanic workshop's site
    When the customer clicks on the contact button
    Then they can write a message to the workshop
  Scenario: Customer Fails to Send Message to Mechanic Workshop
    Given the customer is on the chosen mechanic workshop's site
    When the customer clicks on the contact button
    Then they receive a message saying "The message cannot be empty"
