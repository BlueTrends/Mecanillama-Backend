Feature: Mechanic Service Search
  Scenario: Customer Searches with Filters
    Given the customer is on the main site
    When the customer clicks on the search option
    Then the corresponding filters are applied
  Scenario: No Search Results with Assigned Filters
    Given the customer has applied the corresponding search filters
    When the customer clicks on the search icon
    Then they receive a message saying "No results available"
