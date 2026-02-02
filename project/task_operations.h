#pragma once

#include "task_struct.h"
#include <string>

void searchTasks();
void filterByTime();
void sortTasks();
bool compareByPriority(Task& a, Task& b);
bool compareByDateTime(Task& a, Task& b);
void clearInput();
