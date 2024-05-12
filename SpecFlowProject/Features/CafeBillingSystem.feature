Feature: Cafe Billing System

@Checkout
# Assuming time should be after 19:00 so discount is not applied as part of this test
Scenario: Checkout calculation - regular order - no discount applied
	Given Visitors place an order at '20:27'
		| Starters | Mains | Drinks |
		| 4        | 4     | 4      |
	When Visitors request the final bill
	Then The bill is calculated and is '58.4'£

@Checkout
# Assuming the visitors did not pay the first bill and the final order is summarized
# Otherwise a step to pay the bill will be needed and status property added to the order
Scenario: Checkout calculation - additional order - partial discount applied
	Given Visitors place an order at '17:59'
		| Starters | Mains | Drinks |
		| 1        | 2     | 2      |
	When Visitors request the final bill
	Then The bill is calculated and is '23.3'£
	When Visitors place an order at '20:00'
		| Starters | Mains | Drinks |
		| 0        | 2     | 2      |
	And Visitors request the final bill
	Then The bill is calculated and is '43.7'£

@Checkout
Scenario: Checkout calculation - partial order cancelation - partial discount applied
	Given Visitors place an order at '18:59'
		| Starters | Mains | Drinks |
		| 4        | 4     | 4      |
	When Visitors request the final bill
	Then The bill is calculated and is '55.4'£
	When Visitor cancels the order from '18:59'
		| Starters | Mains | Drinks |
		| 1        | 1     | 1      |
	And Visitors request the final bill
	Then The bill is calculated and is '41.55'£


@Checkout
# Additional check that visitors can cancel the whole order and leave
Scenario: Checkout calculation - full order cancelation - no discount applied
	Given Visitors place an order at '23:59'
		| Starters | Mains | Drinks |
		| 4        | 4     | 4      |
	When Visitors request the final bill
	Then The bill is calculated and is '58.4'£
	When Visitor cancels the order from '23:59'
		| Starters | Mains | Drinks |
		| 4        | 4     | 4      |
	And Visitors request the final bill
	Then The bill is calculated and is '0'£