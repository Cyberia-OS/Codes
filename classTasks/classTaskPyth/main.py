import math
import random

# 1. Копилка
class PiggyBank:
    def __init__(self):
        self.money = 0

    def put(self, amount):
        self.money += amount

    def take(self, amount):
        if amount <= self.money:
            self.money -= amount
        else:
            print(f"Ошибка: мало денег (всего {self.money})")

    def show(self):
        print(f"Копилка: {self.money}")


# 2. Светофор
class TrafficLight:
    def __init__(self):
        self.color = "red"

    def next(self):
        mapping = {"red": "green", "green": "yellow", "yellow": "red"}
        self.color = mapping[self.color]

    def show(self):
        print(f"Светофор: {self.color}")


# 3. Точка
class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def move(self, dx, dy):
        self.x += dx
        self.y += dy

    def distanceTo(self, other):
        return math.sqrt((other.x - self.x) ** 2 + (other.y - self.y) ** 2)

    def show(self):
        print(f"Точка: ({self.x}, {self.y})")


# 4. Термометр
class Thermometer:
    def __init__(self):
        self._celsius = 0.0

    @property
    def celsius(self):
        return self._celsius

    @celsius.setter
    def celsius(self, value):
        if value >= -273.15:
            self._celsius = value
        else:
            print("Ошибка: ниже абсолютного нуля")

    @property
    def fahrenheit(self):
        return self._celsius * 9 / 5 + 32


# 5. Тамагочи
class Pet:
    def __init__(self, name):
        self.name = name
        self.hunger = 50
        self.happiness = 50

    def feed(self):
        self.hunger = max(0, self.hunger - 20)
        self.happiness = min(100, self.happiness + 5)

    def play(self):
        self.happiness = min(100, self.happiness + 15)
        self.hunger = min(100, self.hunger + 10)

    def tick(self):
        self.hunger = min(100, self.hunger + 5)
        self.happiness = max(0, self.happiness - 3)

    @property
    def mood(self):
        if self.happiness > 70: return "счастлив"
        if self.happiness < 40: return "грустен"
        return "нормально"


# Тест 1
bank = PiggyBank()
bank.put(100)
bank.put(50)
bank.take(200)
bank.show()

# Тест 2
light = TrafficLight()
for _ in range(3):
    light.next()
    print(light.color, end=" ")
print()

# Тест 3
p1 = Point(0, 0)
p2 = Point(3, 4)
p1.move(1, 1)
print(f"Расстояние: {p1.distanceTo(p2):.2f}")

# Тест 4
t = Thermometer()
t.celsius = 25
print(f"{t.celsius} C = {t.fahrenheit} F")
t.celsius = -300

# Тест 5
pet = Pet("Тома")
for i in range(1, 4):
    action = random.choice([pet.feed, pet.play, pet.tick])
    action()
    print(f"Шаг {i}: {pet.mood}")
