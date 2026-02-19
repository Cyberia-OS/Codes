from calculator_with_history import CalculatorWithHistory

def test_addition():
    calc = CalculatorWithHistory()
    result = calc.perform_addition(5, 3)
    assert result == 8
    assert calc.get_operation_history() == ["5 + 3 -> 8"]

def test_subtraction():
    calc = CalculatorWithHistory()
    result = calc.perform_subtraction(10, 4)
    assert result == 6
    assert calc.get_operation_history() == ["10 - 4 -> 6"]

def test_history_order():
    calc = CalculatorWithHistory()
    calc.perform_addition(1, 1)
    calc.perform_subtraction(5, 2)
    assert calc.get_operation_history() == ["1 + 1 -> 2", "5 - 2 -> 3"]

if __name__ == "__main__":
    test_addition()
    test_subtraction()
    test_history_order()