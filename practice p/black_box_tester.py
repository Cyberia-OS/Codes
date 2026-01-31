"""
Фреймворк для тестирования методом черного ящика
"""
import subprocess
import time
import os
from datetime import datetime


class BugReport:
    """Класс для создания баг-репортов"""

    def __init__(self):
        self.reports = []
        self.test_count = 0
        self.bug_count = 0

    def add_report(self, test_case, expected, actual, status, bug_type=""):
        """Добавить отчет о тесте"""
        self.test_count += 1
        if status == "FAIL":
            self.bug_count += 1

        report = {
            "id": self.test_count,
            "test_case": test_case,
            "expected": expected,
            "actual": actual,
            "status": status,
            "bug_type": bug_type,
            "timestamp": datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        }
        self.reports.append(report)

    def save_to_file(self, filename="bug_reports.txt"):
        """Сохранить все баг-репорты в файл"""
        with open(filename, 'w', encoding='utf-8') as f:
            f.write("=" * 60 + "\n")
            f.write("ОТЧЕТ О ТЕСТИРОВАНИИ ЧЕРНЫМ ЯЩИКОМ\n")
            f.write("=" * 60 + "\n\n")
            f.write(f"Дата тестирования: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}\n")
            f.write(f"Всего тестов: {self.test_count}\n")
            f.write(f"Найдено багов: {self.bug_count}\n")
            f.write("=" * 60 + "\n\n")

            for report in self.reports:
                f.write(f"Тест #{report['id']}\n")
                f.write(f"Время: {report['timestamp']}\n")
                f.write(f"Тест-кейс: {report['test_case']}\n")
                f.write(f"Ожидаемый результат: {report['expected']}\n")
                f.write(f"Фактический результат: {report['actual']}\n")
                f.write(f"Статус: {report['status']}\n")
                if report['bug_type']:
                    f.write(f"Тип бага: {report['bug_type']}\n")
                f.write("-" * 40 + "\n")

        print(f"\nОтчет сохранен в файл: {filename}")


class BlackBoxTester:
    """Класс для тестирования методом черного ящика"""

    def __init__(self, program_path):
        self.program_path = program_path
        self.bug_report = BugReport()

    def run_test(self, inputs, expected_output, test_name, bug_type=""):
        """Запустить один тест"""
        print(f"\n{'=' * 40}")
        print(f"ТЕСТ: {test_name}")
        print(f"Входные данные: {inputs}")
        print(f"Ожидаемый результат: {expected_output}")

        try:
            # Запускаем программу как subprocess
            process = subprocess.Popen(
                ['python', self.program_path],
                stdin=subprocess.PIPE,
                stdout=subprocess.PIPE,
                stderr=subprocess.PIPE,
                text=True,
                encoding='utf-8'
            )

            # Отправляем входные данные
            input_str = '\n'.join(str(i) for i in inputs) + '\nexit\n'
            stdout, stderr = process.communicate(input=input_str, timeout=5)

            # Анализируем результат
            actual = stdout.strip()

            # Проверяем наличие ошибок
            if stderr:
                actual = f"STDERR: {stderr[:100]}..."

            # Ищем ожидаемый результат в выводе
            if expected_output in actual:
                status = "PASS"
                print("✓ ТЕСТ ПРОЙДЕН")
            else:
                status = "FAIL"
                print("✗ ТЕСТ НЕ ПРОЙДЕН (НАЙДЕН БАГ)")

            print(f"Фактический результат: {actual[:100]}...")

        except subprocess.TimeoutExpired:
            status = "FAIL"
            actual = "ТАЙМАУТ (возможно бесконечный цикл)"
            print("✗ ТЕСТ НЕ ПРОЙДЕН (ТАЙМАУТ)")
        except Exception as e:
            status = "FAIL"
            actual = f"Ошибка выполнения: {str(e)[:100]}"
            print(f"✗ ТЕСТ НЕ ПРОЙДЕН: {e}")

        self.bug_report.add_report(
            test_name,
            expected_output,
            actual,
            status,
            bug_type
        )

        return status == "PASS"

    def run_test_suite(self):
        """Запустить набор тестов"""
        print("\n" + "=" * 60)
        print("НАЧАЛО ТЕСТИРОВАНИЯ МЕТОДОМ ЧЕРНОГО ЯЩИКА")
        print("=" * 60)

        # Тест 1: Нормальное сложение
        self.run_test(
            inputs=["5", "+", "3"],
            expected_output="Результат: 8.0",
            test_name="Сложение положительных чисел",
            bug_type=""
        )

        # Тест 2: Вычитание (баг!)
        self.run_test(
            inputs=["3", "-", "5"],
            expected_output="Результат: -2.0",
            test_name="Вычитание из меньшего числа большего",
            bug_type="Логическая ошибка"
        )

        # Тест 3: Умножение 10*10 (баг!)
        self.run_test(
            inputs=["10", "*", "10"],
            expected_output="Результат: 100.0",
            test_name="Умножение 10 на 10",
            bug_type="Арифметическая ошибка"
        )

        # Тест 4: Деление на 0 (баг!)
        self.run_test(
            inputs=["10", "/", "0"],
            expected_output="ОШИБКА: Деление на ноль!",
            test_name="Деление на ноль",
            bug_type="Обработка исключений"
        )

        # Тест 5: Квадратный корень из отрицательного числа (баг!)
        self.run_test(
            inputs=["-9", "sqrt"],
            expected_output="ОШИБКА: Мнимые числа не поддерживаются!",
            test_name="Квадратный корень из отрицательного числа",
            bug_type="Обработка исключений"
        )

        # Тест 6: Некорректный ввод (баг!)
        self.run_test(
            inputs=["abc", "+", "5"],
            expected_output="ОШИБКА ввода",
            test_name="Ввод букв вместо цифр",
            bug_type="Валидация входных данных"
        )

        # Тест 7: Нормальное деление
        self.run_test(
            inputs=["10", "/", "2"],
            expected_output="Результат: 5.0",
            test_name="Нормальное деление",
            bug_type=""
        )

        # Тест 8: Возведение в степень
        self.run_test(
            inputs=["2", "power", "3"],
            expected_output="Результат: 8.0",
            test_name="Возведение в степень",
            bug_type=""
        )

        # Сохраняем отчет
        self.bug_report.save_to_file()

        # Выводим статистику
        print("\n" + "=" * 60)
        print("СТАТИСТИКА ТЕСТИРОВАНИЯ")
        print("=" * 60)
        print(f"Всего выполнено тестов: {self.bug_report.test_count}")
        print(f"Найдено багов: {self.bug_report.bug_count}")
        print(f"Успешных тестов: {self.bug_report.test_count - self.bug_report.bug_count}")

        if self.bug_report.bug_count > 0:
            print("\nНАЙДЕННЫЕ БАГИ:")
            for report in self.bug_report.reports:
                if report['status'] == 'FAIL':
                    print(f"  - {report['test_case']}: {report['bug_type']}")


def manual_testing():
    """Ручное тестирование для студентов"""
    print("\n" + "=" * 60)
    print("РУЧНОЕ ТЕСТИРОВАНИЕ")
    print("=" * 60)
    print("Инструкция:")
    print("1. Запустите программу buggy_calculator.py")
    print("2. Проводите тесты, вводя различные данные")
    print("3. Фиксируйте найденные баги в баг-репорте")
    print("4. Используйте следующий шаблон для баг-репортов:\n")

    print("ШАБЛОН БАГ-РЕПОРТА:")
    print("-" * 40)
    print("ID: [уникальный номер]")
    print("Название: [краткое описание бага]")
    print("Шаги воспроизведения:")
    print("  1. [шаг 1]")
    print("  2. [шаг 2]")
    print("  3. [шаг 3]")
    print("Ожидаемый результат: [что должно было произойти]")
    print("Фактический результат: [что произошло на самом деле]")
    print("Серьезность: [Critical/Major/Minor]")
    print("Приоритет: [High/Medium/Low]")
    print("Среда: [Windows/Linux, версия Python]")
    print("-" * 40)


if __name__ == "__main__":
    # Автоматическое тестирование
    tester = BlackBoxTester("buggy_calculator.py")
    tester.run_test_suite()

    # Предложение ручного тестирования
    choice = input("\nХотите провести ручное тестирование? (y/n): ")
    if choice.lower() == 'y':
        manual_testing()

    # Запуск тестируемого приложения для ручного теста
    choice2 = input("\nЗапустить калькулятор для ручного тестирования? (y/n): ")
    if choice2.lower() == 'y':
        print("\n" + "=" * 60)
        print("ЗАПУСК ТЕСТИРУЕМОГО ПРИЛОЖЕНИЯ")
        print("=" * 60)
        import buggy_calculator

        buggy_calculator.buggy_calculator()
