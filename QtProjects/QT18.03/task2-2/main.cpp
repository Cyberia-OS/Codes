#include <QApplication>
#include "matrixcols.h"

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    MatrixCols w;
    w.show();
    return a.exec();
}
