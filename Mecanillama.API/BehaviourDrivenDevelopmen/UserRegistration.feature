Feature: User Registration
  Scenario: Missing Required Fields
    Given I am on the registration page
    When I do not fill in the required fields
    And I click the "Register" button
    Then I should see the message: "Please enter the missing fields"
  Scenario: Existing User
    Given I am on the registration page
    And I fill in the required fields with existing user information
    When I click the "Register" button
    Then I should see the message: "The user already exists"
