using System.Reflection.Metadata.Ecma335;
using System.Text;

try
{
Console.WriteLine(Game.GetPrintebleState());  // выводит первоначальную доску

while (Game.g.GetWinner() == Winner.Unfineshed)
{
    int index = int.Parse(Console.ReadLine()); // запрос цифры
    Game.g.MakeMove(index);

    Console.WriteLine();
    Console.WriteLine(Game.GetPrintebleState()); // выводим текущее состояние

}
Console.WriteLine($"Победили {Game.g.GetWinner()}");


}
catch (Exception ex)
{

    Console.WriteLine(ex.Message );
}

Console.ReadKey();


public enum Winner // определяем кто победил
{
    Crosses,
    Zeroes,
    Draw,
    Unfineshed
}
public enum State // выбранный символ
{
    Cross,
    Zero,
    Unset // пустое поле
}

public class Game
{
    public static Game g = new Game();

    /// <summary>
    /// метод возвращает строку для вывода игровых полей
    /// </summary>
    /// <returns></returns>
    public static string GetPrintebleState()
    {
        var sb = new StringBuilder();
        // цикл до 7 т.к. 8 и 9 инкрементировать не надо
        for (int i = 1; i <= 7; i += 3)
        {
            sb.AppendLine("     |     |    ")
                .AppendLine(
                $"  {GetPrintebleChar(i)}  |  {GetPrintebleChar(i + 1)}  |  {GetPrintebleChar(i + 2)}")
                .AppendLine("_____|_____|_____");

        }
        return sb.ToString();
    }

    /// <summary>
    /// возвращает символ по введенной цифре 1..9
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static string GetPrintebleChar(int index)
    {
        State state = g.GetState(index);  // отнимает от индекс 1
        if (state == State.Unset)
            return index.ToString(); // возвратим само число если 
        return state == State.Cross ? "X" : "O";
    }



    private readonly State[] board = new State[9];
    // работаем с одномерными массивами

    public int MoveCounter { get; set; }

    public Game()
    {
        // в начале всем i присвоим Unset
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = State.Unset;
        }

    }

    // СЧЕТЧИК ХОДОВ 0 четный ход крестик 1 нечетный нолик
    /// <summary>
    /// принимает индекс 1..9 возвращает State.Cross или State.Zero
    /// </summary>
    /// <param name="index"></param>
    public void MakeMove(int index)
    {
        board[index - 1] = MoveCounter % 2 == 0 ? State.Cross : State.Zero;
        MoveCounter++;
    }

    // для вывода состояния клетки по индексу
    public State GetState(int index)
    {
        return board[index - 1];
    }
    //определим победителя
    public Winner GetWinner()
    {
        return GetWinner(1, 4, 7, 2, 5, 8, 3, 6, 9,
            1, 2, 3, 4, 5, 6, 7, 8, 9,
            1, 5, 9, 3, 5, 7);
    }

    public Winner GetWinner(params int[] indexes)
    {
        // перебор триплетами
        for (int i = 0; i < indexes.Length; i += 3)
        {

            bool same = AreSame(indexes[i], indexes[i + 1], indexes[i + 2]); // проверка триплетов, что в них один и тот же state
            if (same)
            {
                State state = GetState(indexes[i]); // если сдесь крестик, значит во всех 3х крестики т.к. AreSame вернул true
                if (state != State.Unset)
                {
                    return state == State.Cross ? Winner.Crosses : Winner.Zeroes; // возврат крестиков если крестик, ноликов если нолик
                }

            }
        }
        if (MoveCounter < 9)
            return Winner.Unfineshed;
        return Winner.Draw;
    }
    // вспомогательный метод принимает 3 поля, если все совпадают то true
    private bool AreSame(int a, int b, int c)
    {
        return GetState(a) == GetState(b) && GetState(a) == GetState(c);
    }

}
