//исходный код до рефакторинга
//проблемы: сырые массивы, ручная память, пузырьковая сортировка, плохие имена, дублирование логики, утечка памяти

#include <iostream>
#include <cstring>

#define MAX 100

struct S {
    char nm[50]; //плохое имя, фиксированный буфер  риск переполнения
    int* gr; //сырой указатель утечка памяти
    int gc;  
};

//дублирующееся вычисление среднего нет единой функции
double avg(S s) { //лишняя копия
    double sm = 0;
    for (int i = 0; i < s.gc; i++)
        sm += s.gr[i];
    return sm / s.gc;
}

int main() {
    S arr[MAX];    
    int n;
    std::cin >> n;

    for (int i = 0; i < n; i++) {
        std::cin >> arr[i].nm;  
        std::cin >> arr[i].gc;
        arr[i].gr = new int[arr[i].gc]; //ручное выделение памяти
        for (int j = 0; j < arr[i].gc; j++)
            std::cin >> arr[i].gr[j];
    }

    //пузырьковая сортировка медленно
    for (int i = 0; i < n - 1; i++) {
        for (int j = 0; j < n - i - 1; j++) {
            if (avg(arr[j]) < avg(arr[j + 1])) {  
                S tmp = arr[j];
                arr[j] = arr[j + 1];
                arr[j + 1] = tmp;
            }
        }
    }

    //вывод топ-3
    std::cout << "Top 3:" << std::endl;
    for (int i = 0; i < 3; i++) { //нет проверки а вдруг студентов меньше 3
        std::cout << arr[i].nm << " " << avg(arr[i]) << std::endl;
    }

    std::cout << "All:" << std::endl;
    for (int i = 0; i < n; i++) {
        std::cout << arr[i].nm << " " << avg(arr[i]) << std::endl;
    }

    // Утечка памяти delete никогда не вызывается
    return 0;
}