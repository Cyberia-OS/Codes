class ListStack:
    def __init__(self):
        self.stack = []

    def push(self, x):
        self.stack.append(x)

    def pop(self):
        if not self.is_empty():
            return self.stack.pop()
        return None

    def top(self):
        if not self.is_empty():
            return self.stack[-1]
        return None

    def is_empty(self):
        return len(self.stack) == 0

    def size(self):
        return len(self.stack)


def check_brackets_python(string):
    stack = ListStack()
    brackets = {")": "(", "}": "{", "]": "["}

    for ch in string:
        if ch in brackets.values():
            stack.push(ch)
        elif ch in brackets.keys():
            if stack.is_empty():
                return False
            if stack.pop() != brackets[ch]:
                return False

    return stack.is_empty()


test_str = "{()}[]"
print("Python Result:", check_brackets_python(test_str))
