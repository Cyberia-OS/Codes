#pragma once

#include <string>
#include <vector>

struct Task {
    int id;
    std::string title;
    int priority;
    std::string description;
    std::string date;
    std::string time;
    bool completed;

    Task(int i, std::string t, int p, std::string d, std::string dt, std::string tm);
};

extern std::vector<Task> tasks;
extern int nextId;

void addTask();
void editTask();
void deleteTask();
void showTasks(bool showAll = true, std::string filter = "");
