#include <iostream>
#include <math.h>
using namespace std;

int main()
{
    double a, b, c, d, x, x1, x2; 
    setlocale(0, "Rus");    // ���� ������� ����
    cout << "������� ������������ ���������:\n";   
    cin >> a >> b >> c;   // ���� � ����������
    d = b * b - 4 * a * c;   // ������� �������������
    if (d < 0) {
        cout << "������������ ������ ����, ������� ������ ������������\n";    
        return -1;
    }
    if (d == 0) {
        x = -b / 2 * a;
        cout << "x = " << x << endl;
        return -2;
    }
    x1 = (-b - sqrt(d)) / 2 * a;   
    x2 = (-b + sqrt(d)) / 2 * a;   
    cout << "x1 = " << x1 << endl;  
    cout << "x2 = " << x2 << endl;  
    return 0;
}