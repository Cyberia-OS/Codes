from calculator_service import CalculatorService
from history_service import HistoryService

class CalculatorWithHistory:
    def __init__(self):
        self.calculator = CalculatorService()
        self.history = HistoryService()

    def perform_addition(self, a: int, b: int) -> int:
        result = self.calculator.add(a, b)
        self.history.save_record(f"{a} + {b}", result)
        return result

    def perform_subtraction(self, a: int, b: int) -> int:
        result = self.calculator.subtract(a, b)
        self.history.save_record(f"{a} - {b}", result)
        return result

    def get_operation_history(self) -> list:
        return self.history.get_history()