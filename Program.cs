// See https://aka.ms/new-console-template for more information
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

//rerender();

int boardwidth = 100;
int boardheight = 25;

List<CastPosition> storedcasts = new List<CastPosition>();
List<Enemy> newenemys = new List<Enemy>();

string wall = "#";
string player = "@";
string air = ".";
string enemy01 = "3";

int playerX = 1;
int playerY = 1;
int newX = 0;
int newY = 0;

bool movedhoriz = false;
bool wentdown = false;
bool wentright = false;

string widthstring = "";

List<InstantiatedBlock> newblocks = new List<InstantiatedBlock>();




void rerender()
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(">>> Welcome to the console-game!");
    Console.ForegroundColor = ConsoleColor.White;
}

Console.WriteLine("press any key to begin...");

void ClearConsole()
{
    Console.Clear();
}




//ConsoleHelper.SetCurrentFont("Consolas", 18);
/*
String g = Console.ReadLine().ToString();
if (g == "siema")
{
    Console.WriteLine("no hej!");
}
*/
//Console.WriteLine("> new game");
//Console.WriteLine("> load game");
//Console.WriteLine("> settings");

List<string> menuoptions = new List<string>() { "new game", "load game", "settings" }; //main menu buttons

//char input = Console.ReadKey().KeyChar;

/*
if(input == 'a')
{

} */

int totaloptions = menuoptions.Count - 1;
int selectedindex = 0;
string selectiontext = "";

string getSelectedText()
{
    return selectiontext;
}

bool compareSelection(string targettext, string source)
{
    if (source.Contains(targettext))
    {
        return true;
    }

    return false;
}


Console.Clear();
rerender();
goto begginingskip;

selectionloop:




ConsoleKeyInfo keyinfo = Console.ReadKey();



//Console.WriteLine(keyinfo.Key.ToString());

if (keyinfo.Key == ConsoleKey.UpArrow)
{
    if(selectedindex <= 0)
    {
        selectedindex = totaloptions;
    }
    else
    {
        selectedindex--;
    }

    ClearConsole();
    rerender();
}
else if(keyinfo.Key == ConsoleKey.DownArrow)
{
    if(selectedindex >= totaloptions)
    {
        selectedindex = 0;
    }
    else
    {
        selectedindex++;
    }

    ClearConsole();
    rerender();
}else if(keyinfo.Key == ConsoleKey.Enter)
{
    //Console.ForegroundColor = ConsoleColor.Green;

    if(compareSelection("new game", selectiontext))
    {
        //goes out of the menu screen loop
        goto newgame;
    }
    else
    {
        return;
    }
}
else
{
    return;
}

begginingskip:

int currentindex = 0;
foreach (String s in menuoptions)
{


if (currentindex == selectedindex)
{
        //ClearConsole();
        //rerender();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" >" + s); //selected
        Console.ForegroundColor = ConsoleColor.White;
        selectiontext = s;
    }
else
{
        //ClearConsole();
        //rerender();
        Console.WriteLine("> " + s); //unfocused (not selected)
        
}

    currentindex++;


}

goto selectionloop;

newgame:

ClearConsole();
Console.WriteLine("you started a new game!");

Console.ReadKey();
//game();
game();

goto newgame;



void game()
{
    
    generateGrid();
   
    
    //todo tommorow, good night now!
}




void generateGrid()
{
    ClearConsole();

    //height
    for(int i = 0; i<boardheight; i++)
    {
        widthstring = "";

        for(int j = 0; j<boardwidth; j++)
        {
            

            if(i == 0 || i == boardheight - 1)
            {
                //make a wall
                widthstring += wall;
            }else if(j == 0 || j == boardwidth - 1)
            {
                widthstring += wall;
            }else if (i == playerY && j == playerX)
            {
                widthstring += player;
            }
            else
            {
                widthstring += air;
            }
        }

        Console.WriteLine(widthstring);
    }

    Console.WriteLine("your X position: " + playerX);
    Console.WriteLine("your Y position: " + playerY);

    foreach(InstantiatedBlock block in newblocks)
    {
        setTile(block.Xpos, block.Ypos, block.element);
    }

    foreach (Enemy enemy in newenemys)
    {

        if(detectPlayer(enemy.enemyx, enemy.enemyy, 2) == true)
        {
            //CircleRaycast(enemy.enemyx, enemy.enemyy, 2, true, "@");


            CastPosition newpos;


            Random rnd = new Random();

            if (rnd.Next(1, 3) == 1)
            {
                newpos = tempenemyposition(new CastPosition { castx = enemy.enemyx, casty = enemy.enemyy }, new CastPosition { castx = playerX, casty = playerY }, true);
            }
            else
            {
                newpos = tempenemyposition(new CastPosition { castx = enemy.enemyx, casty = enemy.enemyy }, new CastPosition { castx = playerX, casty = playerY }, false);
                //lets go
            }

            if (checkforelement(newpos.castx, newpos.casty, wall) == false)
            {
                enemy.enemyx = newpos.castx;
                enemy.enemyy = newpos.casty;

                setTile(enemy.enemyx, enemy.enemyy, enemy.apperance);
            }
            else if (checkforelement(enemy.enemyx, enemy.enemyy, player) == true)
            {
                enemy.enemyx = newpos.castx;
                enemy.enemyy = newpos.casty;

                setTile(enemy.enemyx, enemy.enemyy, enemy.apperance);

                //kill the player or damage it
            }
            else if (checkforelement(newpos.castx, newpos.casty, wall) == true)
            {
                setTile(enemy.enemyx, enemy.enemyy, enemy.apperance);
            }

            //yay!

            
        }


    }

    ConsoleKey movekey = Console.ReadKey().Key;

    newX = 0;
    newY = 0;

    newX = playerX;
    newY = playerY;

    wentdown = false;
    wentright = false;
    
    if (movekey == ConsoleKey.UpArrow) //VERTICAL CHECK
    {
        newY = (playerY-1);
        movedhoriz = false;
    }else if (movekey == ConsoleKey.DownArrow)
    {
        newY = (playerY+1);
        movedhoriz = false;
        wentdown = true;
    }

    if (movekey == ConsoleKey.RightArrow)
    {
        newX = (playerX+1);
        movedhoriz = true;
        wentright = true;
    }
    else if (movekey == ConsoleKey.LeftArrow)
    {
        newX = (playerX-1);
        movedhoriz = true;
    }

    //Console.SetCursorPosition(newX, newY);

    //Console.WriteLine(Console.ReadLine());
    //Console.ReadKey();

    /*
    if(!wallChecker(newX, newY))
    {
        if (movekey == ConsoleKey.UpArrow)
        {
            playerY -= 1;
        }

        if (movekey == ConsoleKey.DownArrow)
        {
            playerY += 1;
        }

        if (movekey == ConsoleKey.RightArrow)
        {
            playerX += 1;
        }

        if (movekey == ConsoleKey.LeftArrow)
        {
            playerX -= 1;
        }
    }*/

    //Console.WriteLine(newY);
    
    //Console.ReadKey();

    if(movedhoriz == true && wentright == true)
    {
        foreach (string line in ConsoleReader.ReadFromBuffer(0, (short)playerY, (short)Console.BufferWidth, 1))
        {
            if (!line.Substring(playerX+1, 1).Contains(wall))
            {


                //basic code
                if (movekey == ConsoleKey.UpArrow)
                {
                    playerY -= 1;
                }

                if (movekey == ConsoleKey.DownArrow)
                {
                    playerY += 1;
                }

                if (movekey == ConsoleKey.RightArrow)
                {
                    playerX += 1;
                }

                if (movekey == ConsoleKey.LeftArrow)
                {
                    playerX -= 1;
                }
                //basic code
                break;
            }
            else
            {
                //Console.WriteLine(line);
                //Console.WriteLine("substring: " + line.Substring(newX, 1));
            }
        }
    }
    if (movedhoriz == true && wentright == false)
    {
        foreach (string line in ConsoleReader.ReadFromBuffer(0, (short)(playerY), (short)Console.BufferWidth, 1))
        {
            if (!line.Substring(playerX-1, 1).Contains(wall))
            {


                //basic code
                if (movekey == ConsoleKey.UpArrow)
                {
                    playerY -= 1;
                }

                if (movekey == ConsoleKey.DownArrow)
                {
                    playerY += 1;
                }

                if (movekey == ConsoleKey.RightArrow)
                {
                    playerX += 1;
                }

                if (movekey == ConsoleKey.LeftArrow)
                {
                    playerX -= 1;
                }
                //basic code
                break;
            }
            else
            {
                //Console.WriteLine(line);
                //Console.WriteLine("substring: " + line.Substring(newX, 1));
            }
        }
    }
    else if(movedhoriz == false && wentdown == false)
    {
        foreach (string line in ConsoleReader.ReadFromBuffer(0, (short)(playerY - 1), (short)Console.BufferWidth, (short)(1)))
        {
            

            if (!line.Substring(newX, 1).Contains(wall))
            {
                //Console.WriteLine(line);
                //Console.WriteLine("substring: " + line.Substring(newX, 1));

                //basic code
                if (movekey == ConsoleKey.UpArrow)
                {
                    playerY -= 1;
                }

                if (movekey == ConsoleKey.DownArrow)
                {
                    playerY += 1;
                }

                if (movekey == ConsoleKey.RightArrow)
                {
                    playerX += 1; //neverused
                }

                if (movekey == ConsoleKey.LeftArrow)
                {
                    playerX -= 1; //neverused
                }
                //basic code
                break;
            }
            else
            {
                //Console.WriteLine(line);
                //Console.WriteLine("substring: " + line.Substring(newX, 1));

                //Console.WriteLine(line);
                //Console.WriteLine("substring: " + line.Substring(newX, 1));
            }
        }
    }
    else if (movedhoriz == false && wentdown == true)
    {
        foreach (string line in ConsoleReader.ReadFromBuffer(0, (short)(playerY+1), (short)Console.BufferWidth, (short)(1)))
        {


            if (!line.Substring(playerX, 1).Contains(wall))
            {
                //Console.WriteLine(line + " siemaaaaaa");
                //Console.WriteLine("substring: " + line.Substring(playerX, 1));

                //basic code
                if (movekey == ConsoleKey.UpArrow)
                {
                    playerY -= 1;
                }

                if (movekey == ConsoleKey.DownArrow)
                {
                    playerY += 1;
                }

                if (movekey == ConsoleKey.RightArrow)
                {
                    playerX += 1; //neverused
                }

                if (movekey == ConsoleKey.LeftArrow)
                {
                    playerX -= 1; //neverused
                }
                //basic code
                break;
            }
            else
            {
                //Console.WriteLine(line);
                //Console.WriteLine("substring: " + line.Substring(newX, 1));

                //Console.WriteLine(line);
                //Console.WriteLine("substring: " + line.Substring(playerX, 1));
            }
        }
    }

    //more controls

    if (movekey == ConsoleKey.Spacebar)
    {
        //setTile(playerX, playerY-1, wall);
        //newblocks.Add(new InstantiatedBlock);

        newblocks.Add(new InstantiatedBlock { Xpos = playerX, Ypos = (playerY-1), element = wall}); //debugging
    }

    if (movekey == ConsoleKey.M)
    {
        //setTile(playerX, playerY-1, wall);
        //newblocks.Add(new InstantiatedBlock);

        //newblocks.Add(new InstantiatedBlock { Xpos = playerX, Ypos = (playerY-1), element = wall});
        CircleRaycast(playerX, playerY/* - 1*/, 3, true, "@");
    }

    if (movekey == ConsoleKey.N) //spawns a new default enemy!
    {
        newenemys.Add(new Enemy { enemyx = playerX, enemyy = (playerY - 4), apperance = enemy01});
    }

    //Console.ReadKey(); //REMOVE IT LATER


    generateGrid();

    //Console.Write("siema", 5);
    //Console.Write("{0,3}\b\b\b");

    //Console.ReadKey();
}

bool wallChecker(int targetX, int targetY)
{
    if(ConsoleReader.ReadFromBuffer((short)targetY, (short)targetX, 1, 1).GetEnumerator().Current == wall)
    {
        return true; //DOESNT WORK DONT USE PLS
    }

    return false;
}

void death()
{
    Console.WriteLine("ure dead");

    Console.ReadKey();
    Environment.Exit(0);
}

void setTile(int X, int Y, String element)
{
    foreach (string line in ConsoleReader.ReadFromBuffer(0, (short)Y, (short)Console.BufferWidth, (short)1))
    {
        if(X >= 0 && Y >= 0)
        {
            Console.SetCursorPosition(X + 1, Y); //i want to backspace so i have to go 1 char further to edit the correct letter :)
            Console.Write("\b" + element);
        }

        //nocrash
    }
}

List<CastPosition> detectioncasts = new List<CastPosition>();

bool detectPlayer(int onex, int oney, int detectionradius)
{
    //CastPosition[] siema;
    List<CastPosition> siema2 = new List<CastPosition>();
    //siema = RadiusCircle(onex, oney, detectionradius, false)
    foreach(CastPosition c in RadiusCircle(onex, oney, detectionradius, true))
    {
        siema2.Add(c);
    }

    foreach(CastPosition c2 in siema2)
    {
        if(checkforelement(c2.castx, c2.casty, player))
        {
            return true;
        }
    }

    return false;
}

void CircleRaycast(int x, int y, int radius, bool visualize, string target)
{
    CastPosition[] tempcaster;

    tempcaster = RadiusCircle(x, y, radius, true);

    if (visualize)
    {
        /*
        foreach (CastPosition c in tempcaster)
        {
            //setTile(c.castx, c.casty, "+");
            newblocks.Add(new InstantiatedBlock { Xpos = c.castx, Ypos = c.casty, element = "+" });
            //Console.WriteLine(c.castx + "   " + c.casty);
            //Console.WriteLine("x: " + c.castx + "      y: " + c.casty); unusuable
            
        }*/

        foreach(CastPosition cp in storedcasts)
        {
            setTile(cp.castx, cp.casty, "+");
            newblocks.Add(new InstantiatedBlock { Xpos = cp.castx, Ypos = cp.casty, element = "+" });
        }

        //Console.ReadKey();
    }
    else
    {
        //no
    }

    
}

CastPosition[] RadiusCircle(int startx, int starty, int radius, bool filled)
{
    int zeropointX = startx;
    int zeropointY = starty;
    CastPosition startingPoint;
    CastPosition endingPoint;

    CastPosition[] returnlist = new CastPosition[]{ new CastPosition{ castx = playerX, casty = playerY} };

    for(int i = 0; i<radius; i++)
    {
        zeropointX -= 1;
        zeropointY -= 1;
    }

    startingPoint = new CastPosition { castx = zeropointX, casty = zeropointY };

    zeropointX = startx;
    zeropointY = starty;

    for (int i = 0; i < radius; i++)
    {
        zeropointX += 1;
        zeropointY += 1;
    }

   

    endingPoint = new CastPosition { castx = zeropointX, casty = zeropointY };

    zeropointX = startx;
    zeropointY = starty;

    if (filled == true)
    {
        for(int i = startingPoint.casty; i<endingPoint.casty; i++)
        {
            for(int j = startingPoint.castx; j<endingPoint.castx; j++)
            {
                if(!checkforelement(j, i, "#"))
                {
                    //returnlist.Append(new CastPosition { castx = j, casty = i }).ToArray();
                    returnlist.ToList().Add(new CastPosition { castx = j, casty = i });
                    //newblocks.Add(new InstantiatedBlock { Xpos = castx, Ypos = casty, element = "+" });
                    //Console.WriteLine("x = " + j + "    y = " + i);

                    storedcasts.Add(new CastPosition { castx = j, casty = i });
                }

                //returnlist.Append(new CastPosition { castx = j, casty = i }).ToArray();

                //returnlist.ToList().Add(new CastPosition { castx = j, casty = i });
            }
        }
    }else if(filled == false) //hope it works >.<
    {
        for (int i = startingPoint.casty; i < endingPoint.casty; i++)
        {
            for (int j = startingPoint.castx; j < endingPoint.castx; j++)
            {
                if (!checkforelement(j, i, "#") && (j == startingPoint.castx || j == endingPoint.castx || i == startingPoint.casty || i == endingPoint.casty))
                {
                    //returnlist.Append(new CastPosition { castx = j, casty = i }).ToArray();
                    returnlist.ToList().Add(new CastPosition { castx = j, casty = i });
                    //Console.WriteLine("x = " + j + "    y = " + i);

                    storedcasts.Add(new CastPosition { castx = j, casty = i });
                }
            }
        }
    }

    return returnlist;

}

CastPosition tempenemyposition(CastPosition enemyPos, CastPosition targetPos, bool prioritizeXposition)
{
    //CHECK FOR THE WALLS IN ANOTHER METHOD

    if(prioritizeXposition == true)
    {
        //CALCULATING THE NEXT X POSITION!!!

        if(targetPos.castx < enemyPos.castx)
        {
            return new CastPosition { castx = (enemyPos.castx - 1) , casty = enemyPos.casty};
        }
        else if (targetPos.castx > enemyPos.castx)
        {
            return new CastPosition { castx = (enemyPos.castx + 1), casty = enemyPos.casty };
        }

        //CALCULATING THE NEXT Y POSITION!!!

        if(targetPos.casty < enemyPos.casty)
        {
            return new CastPosition { castx = enemyPos.castx, casty = (enemyPos.casty - 1) };
        }
        else if (targetPos.casty > enemyPos.casty)
        {
            return new CastPosition { castx = enemyPos.castx, casty = (enemyPos.casty + 1) };
        }

        //ELSE THE ENEMY REACHED PLAYER SPOT

        return new CastPosition { castx = enemyPos.castx, casty = enemyPos.casty};
    }
    else
    {

        //CALCULATING THE NEXT Y POSITION!!!

        if (targetPos.casty < enemyPos.casty)
        {
            return new CastPosition { castx = enemyPos.castx, casty = (enemyPos.casty - 1) };
        }
        else if (targetPos.casty > enemyPos.casty)
        {
            return new CastPosition { castx = enemyPos.castx, casty = (enemyPos.casty + 1) };
        }

        //CALCULATING THE NEXT X POSITION!!!

        if (targetPos.castx < enemyPos.castx)
        {
            return new CastPosition { castx = (enemyPos.castx - 1), casty = enemyPos.casty };
        }
        else if (targetPos.castx > enemyPos.castx)
        {
            return new CastPosition { castx = (enemyPos.castx + 1), casty = enemyPos.casty };
        }

        //ELSE THE ENEMY REACHED PLAYER SPOT

        return new CastPosition { castx = enemyPos.castx, casty = enemyPos.casty };
    }


}

bool checkforelement(int x, int y, string element)
{
    foreach (string line in ConsoleReader.ReadFromBuffer(0, (short)y, (short)Console.BufferWidth, 1))
    {
        

        if (x > 0 && y > 0)
        {
            if (line.Substring(x, 1).Contains(element))
            {
                return true;
                break;
            }
            else
            {
                return false;
                break;
            }
        }

        if (x <= 0 || y <= 0)
        {
            return false;
        }
    }

    return false;
}

public class CastPosition{
    public int castx { get; set; }
    public int casty { get; set; }
}


//TESTING ONLY HERE

//siema:

//ConsoleKeyInfo keyinfo = Console.ReadKey();

//Console.WriteLine(keyinfo.Key.ToString());
/*
if (keyinfo.Key == ConsoleKey.DownArrow)
{
    Console.WriteLine("ok!");
}*/

//goto siema;


public static class ConsoleHelper
{

    static void InvokeConsole(short ln)
    {
        //ConsoleRead(ln);
    }

    static void ConsoleRead(short linecount)
    {
        // read 10 lines from the top of the console buffer
        foreach (string line in ConsoleReader.ReadFromBuffer(0, 0, (short)Console.BufferWidth, linecount))
        {
            Console.Write(line);
        }
    }


    private const int FixedWidthTrueType = 54;
    private const int StandardOutputHandle = -11;

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr GetStdHandle(int nStdHandle);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool GetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref FontInfo ConsoleCurrentFontEx);


    private static readonly IntPtr ConsoleOutputHandle = GetStdHandle(StandardOutputHandle);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct FontInfo
    {
        internal int cbSize;
        internal int FontIndex;
        internal short FontWidth;
        public short FontSize;
        public int FontFamily;
        public int FontWeight;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.wc, SizeConst = 32)]
        public string FontName;
    }

    public static FontInfo[] SetCurrentFont(string font, short fontSize = 0)
    {
        //Console.WriteLine("Set Current Font: " + font);

        FontInfo before = new FontInfo
        {
            cbSize = Marshal.SizeOf<FontInfo>()
        };

        if (GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref before))
        {

            FontInfo set = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>(),
                FontIndex = 0,
                FontFamily = FixedWidthTrueType,
                FontName = font,
                FontWeight = 400,
                FontSize = fontSize > 0 ? fontSize : before.FontSize
            };

            // Get some settings from current font.
            if (!SetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref set))
            {
                var ex = Marshal.GetLastWin32Error();
                Console.WriteLine("Set error " + ex);
                throw new System.ComponentModel.Win32Exception(ex);
            }

            FontInfo after = new FontInfo
            {
                cbSize = Marshal.SizeOf<FontInfo>()
            };
            GetCurrentConsoleFontEx(ConsoleOutputHandle, false, ref after);

            return new[] { before, set, after };
        }
        else
        {
            var er = Marshal.GetLastWin32Error();
            Console.WriteLine("Get error " + er);
            throw new System.ComponentModel.Win32Exception(er);
        }
    }
}

public class ConsoleReader
{
    public static IEnumerable<string> ReadFromBuffer(short x, short y, short width, short height)
    {
        IntPtr buffer = Marshal.AllocHGlobal(width * height * Marshal.SizeOf(typeof(CHAR_INFO)));
        if (buffer == null)
            throw new OutOfMemoryException();

        try
        {
            COORD coord = new COORD();
            SMALL_RECT rc = new SMALL_RECT();
            rc.Left = x;
            rc.Top = y;
            rc.Right = (short)(x + width - 1);
            rc.Bottom = (short)(y + height - 1);

            COORD size = new COORD();
            size.X = width;
            size.Y = height;

            const int STD_OUTPUT_HANDLE = -11;
            if (!ReadConsoleOutput(GetStdHandle(STD_OUTPUT_HANDLE), buffer, size, coord, ref rc))
            {
                // 'Not enough storage is available to process this command' may be raised for buffer size > 64K (see ReadConsoleOutput doc.)
                //throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            IntPtr ptr = buffer;
            for (int h = 0; h < height; h++)
            {
                StringBuilder sb = new StringBuilder();
                for (int w = 0; w < width; w++)
                {
                    CHAR_INFO ci = (CHAR_INFO)Marshal.PtrToStructure(ptr, typeof(CHAR_INFO));
                    char[] chars = Console.OutputEncoding.GetChars(ci.charData);
                    sb.Append(chars[0]);
                    ptr += Marshal.SizeOf(typeof(CHAR_INFO));
                }
                yield return sb.ToString();
            }
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct CHAR_INFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] charData;
        public short attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct COORD
    {
        public short X;
        public short Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SMALL_RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct CONSOLE_SCREEN_BUFFER_INFO
    {
        public COORD dwSize;
        public COORD dwCursorPosition;
        public short wAttributes;
        public SMALL_RECT srWindow;
        public COORD dwMaximumWindowSize;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadConsoleOutput(IntPtr hConsoleOutput, IntPtr lpBuffer, COORD dwBufferSize, COORD dwBufferCoord, ref SMALL_RECT lpReadRegion);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);
}

[System.Serializable]
public class InstantiatedBlock
{
    public string element { get; set; }
    public int Xpos { get; set; }
    public int Ypos { get; set; }
}

[System.Serializable] //this is required for all mt-vars
public class Enemy
{
    public int enemyx { get; set; }
    public int enemyy { get; set; }
    public string apperance { get; set; }
    //public float damage { get; set; } //not used for now as theres no health system

}