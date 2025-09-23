#include <iostream>  
using namespace std;

int main() {
    int month, day;
    setlocale(0, "Rus");
 
    cout << "Введите номер месяца (1-12): ";
    cin >> month;
    cout << "Введите день месяца: ";
    cin >> day;
 
    if (month < 1 || month > 12) {
        cout << "Ошибка: Введите правильный номер месяца!" << endl;
        return 1;
    }
  
    string season;
    if (month == 12 && day >= 22 || month <= 2 && day <= 20) {
        season = "Зима";
    }
    else if (month == 3 && day >= 21 || month <= 5 && day <= 20) {
        season = "Весна";
    }
    else if (month == 6 && day >= 21 || month <= 8 && day <= 22) {
        season = "Лето";
    }
    else {
        season = "Осень";
    }

    cout << "\nВремя года: " << season << endl;

}
