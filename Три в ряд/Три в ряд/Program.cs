using System;

namespace Три_в_ряд
{
    public class Point {
        int x,
            y;
        public Point(int X, int Y) { // тип данных поинт для удобного использования координат внутри матрицы
            x = X;
            y = Y;
        }

        public void setXY(int X, int Y) {
            x = X;
            y = Y;
        }
        public int getX() {
            return x;
        }
        public int getY() {
            return y;
        }
    }

    public class Token {  // токен, объект перемещаемый игроком внутри матрицы
        int adresX,
            adresY;
        public char type;
        public int getX() {
            return adresX;
        }
        public int getY() {
            return adresY;
        }
        private char letterEror() { // леттерЭрор функция предусмотренная для защиты от появления кристалов не предусмотренного цвета
            Random r = new Random();
            switch (r.Next() % 6)
            {
                case 0:
                    return 'A';
                case 1:
                    return 'B';
                case 2:
                    return 'C';
                case 3:
                    return 'D';
                case 4:
                    return 'E';
                case 5:
                    return 'F';
            }
            return 'A';
        }
        public Token(int x, int y, char letter) {
            adresX = x;
            adresY = y;
            if (letter == 'A' || letter == 'B' || letter == 'C' || letter == 'D' || letter == 'E' || letter == 'F' || letter == 'P') { // дополнительный цвет кристала P обозначающий
                type = letter;                                                                                                            // пустой кристал
            }
            else {
                type = letterEror(); // в случае если сторонний разработчик попытается создать кристал несуществующего цвета
            }                           // несуществующий цвет будет заменёт случайным из сущиствующих цветов
        }

        public Token(Token t) { //конструктор для замены оператора присваивания
            adresX = t.adresX;
            adresY = t.adresY;
            type = t.type;
        }
    }

    public class Field{                         // класс поле предусматривающий игровое поле
        Token[,] tokens = new Token[10,10];
        public Field() { }
        public void init() {
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    tokens[i, j] = new Token(i, j, (char)r.Next('A', 'G'));
                }
            }
        } //заполнение

        public int tick() {            // Функция внутри которой происходит обработка команды игрока и вызывается функцияобработки хода игрока
            Console.Write("Введите команду:");
            string str = Console.ReadLine();
            char [] comand = new char[4];
            int j = 0;
            int controller = 0;
            for (int i = 0; i < str.Length; i++) { //Сипарируем текстовую команду на фрагменты
                if (str[i] == ' ' && controller<=2) {
                    comand[j] = str[i - 1];
                    j++;
                    controller = 0;
                }
                if (i == str.Length - 1) { //Для последнего элемента
                    comand[j] = str[i];
                }
                if (controller >= 2) { // Для проверки верности формата введённой команды
                    comand[0] = 'w';
                }
                controller++;
            }
            if (comand[0] == 'w') {
                Console.WriteLine("Команда введена в неверном формате");
                return 0;
            }
            if (comand[0] == 'q') {
                return -1;
            }
            if (comand[0] == 'r') {
                mix();
                return 0;
            }
            if (comand[0] == 'm') {
                Point p1 = new Point(Convert.ToInt32(comand[1].ToString()), Convert.ToInt32(comand[2].ToString()));
                if (p1.getX() >= 10 || p1.getX() < 0) {
                    Console.WriteLine("Команда введена в неверном формате");
                    return 0;
                }
                if (p1.getY() >= 10 || p1.getY() < 0) {
                    Console.WriteLine("Команда введена в неверном формате");
                    return 0;
                }
                if (comand[3]!= 'l' && comand[3] != 'r' && comand[3] != 'u' && comand[3] != 'd') {
                    Console.WriteLine("Команда введена в неверном формате");
                    return 0;
                }
                Point p2 = new Point(0,0);
                switch (comand[3]) {
                    case 'l':
                        if (p1.getY()==0) {
                            Console.WriteLine("Сдвинуть невозможно");
                            return 0;
                        }
                        p2 = new Point(p1.getX(),p1.getY()-1);
                        break;
                    case 'r':
                        if (p1.getY() == 9)
                        {
                            Console.WriteLine("Сдвинуть невозможно");
                            return 0;
                        }
                        p2 = new Point(p1.getX(), p1.getY() + 1);
                        break;
                    case 'u':
                        if (p1.getX() == 0)
                        {
                            Console.WriteLine("Сдвинуть невозможно");
                            return 0;
                        }
                        p2 = new Point(p1.getX()-1, p1.getY());
                        break;
                    case 'd':
                        if (p1.getX() == 9)
                        {
                            Console.WriteLine("Сдвинуть невозможно");
                            return 0;
                        }
                        p2 = new Point(p1.getX() + 1, p1.getY());
                        break;
                }
                move(p1, p2);
               
            }
            return 0;
        } // выполнение действий на поле

        bool check() { // функция проверяющая поле на совпадения 3-х в ряд
            int count = 0;
            bool retSituation = false;
            for (int i = 0; i < 10; i++) {
                Token t = new Token(tokens[i, 0]);
                for (int j = 0; j < 10; j++) {
                    if (t.type != tokens[i, j].type) {
                        if (count >= 3) {
                            
                            for (int g = t.getY(); g < t.getY() + count; g++) {
                                tokens[i, g] = new Token(i,g, 'P');

                            }
                            if (count >=5) { //дальнейшее расширение функционала под особые фишки
                            
                            }
                            if (count >= 7) { 
                            
                            }
                            retSituation = true;
                            
                        }
                        t = new Token(tokens[i, j]);
                        count = 0;
                    }
                    count++;
                }
            }
            for (int j = 0; j < 10; j++) { 
            Token t = new Token(tokens[0, j]);
                for (int i = 0; i < 10; i++){
                    if (t.type != tokens[i, j].type)
                    {
                        if (count >= 3)
                        {

                            for (int g = t.getX(); g < t.getX() + count; g++)
                            {
                                tokens[g, j] = new Token(g,j,'P');
                            }
                            if (count >= 5)
                            { //дальнейшее расширение функционала под особые фишки

                            }
                            if (count >= 7)
                            {

                            }
                            retSituation = true;
                        }
                        t = new Token(tokens[i, j]);
                        count = 0;
                    }
                    count++;
                }
                }
            return retSituation;
        }

        void fall() { // метод реализующий падение фишек находящихся над пустыми клеткам
            Token t;
            for (int j = 0; j<10;j++) {
                for (int i = 0; i < 10; i++) {
                    if (tokens[i, j].type == 'P' && i != 0) {
                        if (tokens[i-1,j].type!='P') {
                            t = new Token(tokens[i - 1, j]);
                            tokens[i - 1, j] = new Token(tokens[i, j]);
                            tokens[i, j] = new Token(t);
                            i = 0;
                        }
                    }
                }
            }
            Random r = new Random();
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    if (tokens[i, j].type == 'P') {
                        tokens[i, j] = new Token(i, j, (char)r.Next('A', 'G'));
                    }
                }
            }
        }
        public void move(Point from, Point to) { // метод реализующий ход игрока
            
            Token t = new Token(tokens[to.getX(), to.getY()]);
            tokens[to.getX(), to.getY()] = new Token(tokens[from.getX(),from.getY()]);
            tokens[from.getX(), from.getY()] = new Token(t);
            if (check() == true)
            {
                Console.Clear();
                fall();
                dump();
            }
            else {
                t = new Token(tokens[to.getX(), to.getY()]);
                tokens[to.getX(), to.getY()] = new Token(tokens[from.getX(), from.getY()]);
                tokens[from.getX(), from.getY()] = new Token(t);
            }
        } //выполнение хода игрока

        public void mix() {
            init();
        }// перемешивание поля

        public void dump() {
            Console.Write("\t");
            for (int i = 0; i < 10; i++) {
               
                    Console.Write($"{i}\t");
                
            }
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine();
                Console.Write($"{i}\t");
                for (int j = 0; j < 10; j++)
                {
                    Console.Write($"{tokens[i,j].type}\t");
                }
            }

        } //отображение поля
    }
    class Program
    {
        static void Main(string[] args)
        {
            int a = 0;
            Field f = new Field();
            f.init();
            Console.WriteLine("Это игра три в ряд.\n Для того чтобы играть напишите свою команду, к примеру M 0 0 r(Что означает 'Moove' сдвинуть фишку в позиции [0, 0], в право).\n Присудствуют команды:\n u - ВВЕРХ;\n d - ВНИЗ;\n l- ВЛЕВО;\n r - ВПРАВО.\n Чтобы перемешать поле введите r.\n Чтобы закончить игру введите q.\n Чтобы начать играть нажмите Enter.");
            Console.ReadLine();
            f.dump();
            while (a ==0) {
                a = f.tick();
            }

        }
    }
}
