using Cairo;
using Gdk;
using Gtk;
using System;

using Color = Gdk.Color;
using Window = Gtk.Window;
using System.Threading.Tasks;

public class GameView : Window
{

    Game game;

    public GameView(Game game) : base("There"){

        this.game = game;
        this.game.changed += QueueDraw;

        AddEvents((int)(EventMask.KeyPressMask | EventMask.ButtonPressMask));
        Resize(40 * game.rows, 40 * game.column);

        VBox vbox = new VBox();
        Button b = new Button("button");
        b.Clicked += clicked;
        vbox.PackStart(b, false, false, 0);
        this.Add(vbox);

    }
    void clicked(object sender, EventArgs e)
    {
        //code
        QueueDraw();
    }
    static void fillRectangle(Context c, int x, int y, int width,int height,
        int player)
    {
        c.Rectangle(x, y, width, height);
        c.SetSourceRGB(0.0, 0.0, 0.0);
        c.Stroke();

        if (player == 1)
        {
            c.SetSourceRGB(0.0, 0.0, 0.0);
        }
        else
        {
            c.SetSourceRGB(256.0, 256.0, 256.0);
        }

        c.Rectangle(x, y, width, height);
        c.Fill();
    }

    protected override bool OnExposeEvent(EventExpose e)
    {
        using (Context c = CairoHelper.Create(GdkWindow))
        {
            for (int i = 0; i < this.game.column; i++)
            {
                for (int j = 0; j < this.game.rows; j++)
                {
                    fillRectangle(c, j * 40, i * 40, 40, 40,
                        this.game.gameArea[i, j]);
                }
            }
        }
        return true;
    }

    protected override bool OnDeleteEvent(Event ev){
        Application.Quit();
        return true;
    }

    public static void run(Game game)
    {
        Application.Init();
        GameView v = new GameView(game);
        v.ShowAll();
        Application.Run();
    }
}
