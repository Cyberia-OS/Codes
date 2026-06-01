using System;
using System.Collections.Generic;
using System.Threading;

//гонки
public delegate void CarAction(Car car);

//Car
public class Car
{
    public string Name { get; private set; }
    public double Position { get; private set; }
    private Random rnd = new Random();
    private int minSpeed, maxSpeed;
    public event CarAction Finish;

    public Car(string name, int minSpeed, int maxSpeed)
    {
        Name = name;
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;
    }

    public void Move()
    {
        int speed = rnd.Next(minSpeed, maxSpeed + 1);
        Position += speed;
        Console.WriteLine($"{Name}: скорость {speed}, позиция {Position:F1}");
    }

    public void OnFinish()
    {
        Finish?.Invoke(this);
    }
}

public static class RacingGame
{
    const double FINISH = 100;

    public static void Start()
    {
        var cars = new List<Car>
        {
            new Car("Спорткар", 15, 25),
            new Car("Легковая", 10, 18),
            new Car("Грузовик", 5, 12),
            new Car("Автобус", 4, 10)
        };

        foreach (var car in cars)
            car.Finish += c => Console.WriteLine($"\n{c.Name} финишировал и победил!");

        CarAction onStart = car => Console.WriteLine($"{car.Name} на старте.");
        foreach (var car in cars)
            onStart(car);

        Console.WriteLine("\n--- Гонка началась! ---");
        Thread.Sleep(300);

        bool raceOver = false;
        while (!raceOver)
        {
            foreach (var car in cars)
            {
                car.Move();
                if (car.Position >= FINISH)
                {
                    car.OnFinish();
                    raceOver = true;
                    break;
                }
            }
            Thread.Sleep(200);
        }
    }
}

//карточная игра 
public enum Suit { Черви, Бубны, Трефы, Пики }

public enum Rank
{
    Шесть = 6, Семь, Восемь, Девять, Десять,
    Валет = 11, Дама, Король, Туз
}

public class Card
{
    public Suit Suit { get; }
    public Rank Rank { get; }
    public Card(Suit suit, Rank rank) { Suit = suit; Rank = rank; }
    public override string ToString() => $"{RankToString(Rank)} {Suit}";

    private string RankToString(Rank r)
    {
        switch (r)
        {
            case Rank.Валет: return "Валет";
            case Rank.Дама: return "Дама";
            case Rank.Король: return "Король";
            case Rank.Туз: return "Туз";
            default: return ((int)r).ToString();
        }
    }
}

public class Player
{
    public string Name { get; }
    public Queue<Card> Cards { get; }
    public Player(string name) { Name = name; Cards = new Queue<Card>(); }
}

public static class Game
{
    public static Player Start(List<Player> players)
    {
        //тасовка колоды
        var deck = new List<Card>();
        foreach (Suit s in Enum.GetValues(typeof(Suit)))
            for (int r = 6; r <= 14; r++)
                deck.Add(new Card(s, (Rank)r));

        Random rng = new Random();
        int n = deck.Count;
        while (n > 1) { n--; int k = rng.Next(n + 1); var t = deck[k]; deck[k] = deck[n]; deck[n] = t; }

        //раздача
        int idx = 0;
        foreach (var c in deck) { players[idx].Cards.Enqueue(c); idx = (idx + 1) % players.Count; }

        for (int round = 1; round <= 3; round++)
        {
            Console.WriteLine($"\n--- Раунд {round} ---");
            var roundCards = new Dictionary<Player, Card>();
            foreach (var p in players)
            {
                if (p.Cards.Count > 0)
                {
                    Card c = p.Cards.Dequeue();
                    roundCards[p] = c;
                    Console.WriteLine($"{p.Name} -> {c}");
                }
            }

            if (roundCards.Count == 0) 
                break;

            Player winner = null;
            int maxRank = -1;
            foreach (var pair in roundCards)
            {
                int rank = (int)pair.Value.Rank;
                if (rank > maxRank) { maxRank = rank; winner = pair.Key; }
                else if (rank == maxRank && players.IndexOf(pair.Key) < players.IndexOf(winner))
                    winner = pair.Key;
            }

            foreach (var c in roundCards.Values) winner.Cards.Enqueue(c);
            Console.WriteLine($"{winner.Name} забирает карты (теперь {winner.Cards.Count})");
            Thread.Sleep(500);
        }

        //победитель по наибольшему числу карт
        Player finalWinner = players[0];
        for (int i = 1; i < players.Count; i++)
        {
            if (players[i].Cards.Count > finalWinner.Cards.Count)
                finalWinner = players[i];
        }
        Console.WriteLine($"\nПобедитель: {finalWinner.Name} ({finalWinner.Cards.Count} карт)");
        return finalWinner;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("гонки\n");
        RacingGame.Start();

        Console.WriteLine("карточная игра\n");
        var players = new List<Player> { new Player("Иван"), new Player("Ольга") };
        Game.Start(players);

    }
}