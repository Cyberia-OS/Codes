#include <iostream>
using namespace std;


void Triangle(int a, int b, int c) {

	setlocale(0, "Rus");
	if (a + b == c || a + c <= b || b + c <= a) {
		cout << "�� �����������" << endl;
	}
	else if (a == b && b == c) {
		cout << "��������������" << endl;
	}
	else if (a == b || a == c || b == c) {
		cout << "��������������" << endl;
	}
	else {
		cout << "��������������" << endl;
	}
}

int main() {
	int a, b, c;

	cout << "������� ������� ������������ (a, b, c): ";
	cin >> a >> b >> c;

	Triangle(a, b, c);

	return 0;
}
