import asyncio
from typing import Protocol, Dict

#задача 1 
class Timer:
    def __init__(self, seconds):
        self.seconds = seconds
        self.is_paused = False
        self.on_tick = None
        self.on_finish = None

    def pause(self): self.is_paused = True
    def resume(self): self.is_paused = False

    async def start(self):
        while self.seconds >= 0:
            if not self.is_paused:
                if self.seconds > 0 and self.on_tick: self.on_tick(self.seconds)
                elif self.seconds == 0 and self.on_finish: self.on_finish()
                self.seconds -= 1
            else:
                print("Таймер на паузе")
            await asyncio.sleep(1)

def handle_tick(remaining): print(f"Осталось секунд: {remaining}")
def handle_finish(): print("Время вышло")

#задчаа 2
class Observer(Protocol):
    def update(self, temperature: float) -> None: ...

class WeatherStation:
    def __init__(self):
        self.__temperature = 0.0
        self.__observers = []

    def add_observer(self, observer: Observer): self.__observers.append(observer)
    def remove_observer(self, observer: Observer):
        if observer in self.__observers: self.__observers.remove(observer)

    def set_temperature(self, temp: float):
        self.__temperature = temp
        for observer in list(self.__observers): observer.update(self.__temperature)

class Display:
    def update(self, temp: float): print(f"Дисплей: {temp} C")

class Logger:
    def update(self, temp: float): print(f"Логгер: {temp} C")

class Alarm:
    def __init__(self, threshold: float): self.threshold = threshold
    def update(self, temp: float):
        if temp > self.threshold: print(f"Сигнализация: Превышен порог {temp} C")

#задача 4
class Operation(Protocol):
    def apply(self, a: float, b: float) -> float: ...

class Add:
    def apply(self, a: float, b: float) -> float: return a + b

class Sub:
    def apply(self, a: float, b: float) -> float: return a - b

class CalculatorCore:
    def __init__(self):
        self.__operations: Dict[str, Operation] = {}

    def register_operation(self, name: str, op: Operation): self.__operations[name] = op
    def calculate(self, name: str, a: float, b: float) -> float:
        if name in self.__operations: return self.__operations[name].apply(a, b)
        print("Операция не поддерживается")
        return 0.0

async def main():
    print("Тест 1: Таймер")
    t = Timer(3)
    t.on_tick = handle_tick
    t.on_finish = handle_finish
    await t.start()

    print("\nТест 2: Погодная станция")
    station = WeatherStation()
    disp = Display()
    logger = Logger()
    alarm = Alarm(30.0)
    station.add_observer(disp)
    station.add_observer(logger)
    station.add_observer(alarm)
    station.set_temperature(25.5)
    station.set_temperature(32.0)
    station.remove_observer(logger)
    station.set_temperature(22.0)

    print("\nТест 4: Калькулятор")
    calc = CalculatorCore()
    calc.register_operation("+", Add())
    calc.register_operation("-", Sub())
    print(f"5 + 3 = {calc.calculate('+', 5, 3)}")
    print(f"10 - 4 = {calc.calculate('-', 10, 4)}")

if __name__ == "__main__":
    asyncio.run(main())
