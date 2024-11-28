//*****************************************************************************
//** 2290. Minimum Obstacle Removal to Reach Corner    leetcode              **
//*****************************************************************************

// Structure to represent a Point
typedef struct Point
{
    int row;
    int column;
    int removedObstacles;
} Point;

// A deque structure to support push and pop operations
typedef struct Deque
{
    Point* data;
    int front;
    int back;
    int capacity;
} Deque;

// Function prototypes
Deque* createDeque(int capacity);
void destroyDeque(Deque* deque);
void pushFront(Deque* deque, Point point);
void pushBack(Deque* deque, Point point);
Point popFront(Deque* deque);
bool isDequeEmpty(Deque* deque);
bool isInMatrix(int row, int column, int rows, int cols);
int minimumObstacles(int** grid, int gridSize, int* gridColSize);

// Implementation of deque functions
Deque* createDeque(int capacity)
{
    Deque* deque = (Deque*)malloc(sizeof(Deque));
    deque->data = (Point*)malloc(sizeof(Point) * capacity);
    deque->front = 0;
    deque->back = 0;
    deque->capacity = capacity;
    return deque;
}

void destroyDeque(Deque* deque)
{
    free(deque->data);
    free(deque);
}

void pushFront(Deque* deque, Point point)
{
    deque->front = (deque->front - 1 + deque->capacity) % deque->capacity;
    deque->data[deque->front] = point;
}

void pushBack(Deque* deque, Point point)
{
    deque->data[deque->back] = point;
    deque->back = (deque->back + 1) % deque->capacity;
}

Point popFront(Deque* deque)
{
    Point point = deque->data[deque->front];
    deque->front = (deque->front + 1) % deque->capacity;
    return point;
}

bool isDequeEmpty(Deque* deque)
{
    return deque->front == deque->back;
}

bool isInMatrix(int row, int column, int rows, int cols)
{
    return row >= 0 && row < rows && column >= 0 && column < cols;
}

// Main function to find the minimum obstacles
int minimumObstacles(int** grid, int gridSize, int* gridColSize)
{
    int rows = gridSize;
    int cols = gridColSize[0];

    int moves[4][2] = {{-1, 0}, {1, 0}, {0, -1}, {0, 1}};
    Deque* queue = createDeque(rows * cols);
    int** minRemovedObstacles = (int**)malloc(rows * sizeof(int*));

    for (int i = 0; i < rows; ++i)
    {
        minRemovedObstacles[i] = (int*)malloc(cols * sizeof(int));
        for (int j = 0; j < cols; ++j)
        {
            minRemovedObstacles[i][j] = INT_MAX;
        }
    }

    Point start = {0, 0, grid[0][0]};
    pushBack(queue, start);
    minRemovedObstacles[0][0] = grid[0][0];

    while (!isDequeEmpty(queue))
    {
        Point current = popFront(queue);

        if (current.row == rows - 1 && current.column == cols - 1)
        {
            break;
        }

        for (int i = 0; i < 4; ++i)
        {
            int nextRow = current.row + moves[i][0];
            int nextColumn = current.column + moves[i][1];

            if (isInMatrix(nextRow, nextColumn, rows, cols) &&
                minRemovedObstacles[nextRow][nextColumn] > current.removedObstacles + grid[nextRow][nextColumn])
            {
                Point next = {nextRow, nextColumn, current.removedObstacles + grid[nextRow][nextColumn]};
                minRemovedObstacles[nextRow][nextColumn] = next.removedObstacles;

                if (grid[nextRow][nextColumn] == 0)
                {
                    pushFront(queue, next);
                }
                else
                {
                    pushBack(queue, next);
                }
            }
        }
    }

    int result = minRemovedObstacles[rows - 1][cols - 1];

    // Free allocated memory
    for (int i = 0; i < rows; ++i)
    {
        free(minRemovedObstacles[i]);
    }
    free(minRemovedObstacles);
    destroyDeque(queue);

    return result;
}