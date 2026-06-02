#include <QCoreApplication>
#include <QTextStream>
#include <QVector>

int main(int argc, char *argv[])
{
    QCoreApplication a(argc, argv);

// Включаем поддержку UTF-8 в консоли Windows на системном уровне
#ifdef Q_OS_WIN
    system("chcp 65001 > nul");
#endif

    QTextStream in(stdin);
    QTextStream out(stdout);

    // Универсальный способ настройки UTF-8 для Qt 5 и Qt 6
#if QT_VERSION >= QT_VERSION_CHECK(6, 0, 0)
    in.setEncoding(QStringConverter::Utf8);
    out.setEncoding(QStringConverter::Utf8);
#else
    in.setCodec("UTF-8");
    out.setCodec("UTF-8");
#endif

    out << "=== Вывод элементов по чётным и нечётным индексам ===\n\n";
    out << "Введите размер массива N: ";
    out.flush();
    int n = in.readLine().toInt();

    if (n <= 0) {
        out << "Ошибка: N должно быть больше 0.\n";
        out.flush();
        return 1;
    }

    // Заполнение вектора пользователем
    QVector<int> vec;
    out << "Введите " << n << " целых чисел:\n";
    out.flush();
    for (int i = 0; i < n; i++) {
        out << "  vec[" << i << "] = ";
        out.flush();
        vec.append(in.readLine().toInt());
    }

    // Вывод исходного массива
    out << "\nИсходный массив: ";
    for (int i = 0; i < vec.size(); i++)
        out << vec[i] << " ";
    out << "\n";

    // Элементы с чётными индексами: 0, 2, 4, ...
    out << "\nЭлементы с чётными индексами (0, 2, 4, ...):\n";
    for (int i = 0; i < vec.size(); i += 2)
        out << "  vec[" << i << "] = " << vec[i] << "\n";

    // Элементы с нечётными индексами: 1, 3, 5, ...
    out << "\nЭлементы с нечётными индексами (1, 3, 5, ...):\n";
    for (int i = 1; i < vec.size(); i += 2)
        out << "  vec[" << i << "] = " << vec[i] << "\n";

    out.flush();
    return 0;
}
