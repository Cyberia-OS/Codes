import sys
import math

class FixedCalculator:
    def __init__(self):
        self.history = []

    def add(self, a: float, b: float) -> float:
        return a + b

    def subtract(self, a: float, b: float) -> float:
        return a - b

    def multiply(self, a: float, b: float) -> float:
        return a * b

    def divide(self, a: float, b: float) -> float:
        if b == 0:
            raise ZeroDivisionError("Деление на ноль!")
        return a / b

    def power(self, a: float, b: float) -> float:
        #проверка на комплексные числа
        if a < 0 and b != int(b):
            raise ValueError("Возведение отрицательного числа в дробную степень не поддерживается")
        return a ** b

    def sqrt(self, a: float) -> float:
        if a < 0:
            raise ValueError("Квадратный корень из отрицательного числа не поддерживается")
        return math.sqrt(a)

    def save_to_history(self, operation: str, result: float):
        self.history.append(f"{operation} -> {result}")

    def show_history(self):
        if not self.history:
            print("История пуста.")
        else:
            print("=== История операций ===")
            for record in self.history:
                print(record)

def main():
    calc = FixedCalculator()
    print("=" * 50)
    print("УЛУЧШЕННЫЙ КАЛЬКУЛЯТОР")
    print("=" * 50)
    print("Доступные операции: +, -, *, /, power, sqrt")
    print("Для выхода введите 'exit'")
    print("Для просмотра истории введите 'history'")
    print("=" * 50)

    while True:
        try:
            cmd = input("\nВведите первое число (или команду): ").strip()
            if cmd.lower() == 'exit':
                break
            if cmd.lower() == 'history':
                calc.show_history()
                continue

            try:
                num1 = float(cmd)
            except ValueError:
                print("ОШИБКА: Неверный формат числа. Попробуйте снова.")
                continue

            op = input("Введите операцию (+, -, *, /, power, sqrt): ").strip()
            if op == 'sqrt':
                result = calc.sqrt(num1)
                calc.save_to_history(f"sqrt({num1})", result)
                print(f"Результат: {result}")
                continue

            num2_input = input("Введите второе число: ").strip()
            try:
                num2 = float(num2_input)
            except ValueError:
                print("ОШИБКА: Неверный формат второго числа. Попробуйте снова")
                continue

            if op == '+':
                result = calc.add(num1, num2)
                operation_str = f"{num1} + {num2}"
            elif op == '-':
                result = calc.subtract(num1, num2)
                operation_str = f"{num1} - {num2}"
            elif op == '*':
                result = calc.multiply(num1, num2)
                operation_str = f"{num1} * {num2}"
            elif op == '/':
                result = calc.divide(num1, num2)
                operation_str = f"{num1} / {num2}"
            elif op == 'power':
                result = calc.power(num1, num2)
                operation_str = f"{num1} ^ {num2}"
            else:
                print("ОШИБКА: Неизвестная операция.")
                continue

            calc.save_to_history(operation_str, result)
            print(f"Результат: {result}")

        except ZeroDivisionError as e:
            print(f"ОШИБКА: {e}")
        except ValueError as e:
            print(f"ОШИБКА: {e}")
        except OverflowError:
            print("ОШИБКА: Переполнение числа")
        except Exception as e:
            print(f"Неожиданная ошибка: {e}")

    print("Работа калькулятора завершена.")

if __name__ == "__main__":
    main()
