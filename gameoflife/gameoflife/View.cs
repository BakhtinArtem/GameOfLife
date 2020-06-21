using Cairo;
using Gdk;
using Gtk;
using System;

using Color = Gdk.Color;
using Window = Gtk.Window;
using System.Threading;
using System.Diagnostics;

public class GameView : Window
{
    Game game;
    DrawingArea area;
    bool simulation = false;
    int scale = 1;
    uint generationSpeed = 500;

    public GameView(Game game) : base("GameOfLife")
    {
        this.game = game;

        AddEvents((int)(EventMask.KeyPressMask | EventMask.ButtonPressMask
            | EventMask.PointerMotionMask));
        Resize(this.game.column * 10, this.game.rows * 10 + 70);

        //box with start/stop and clear button
        VBox vbox = new VBox();
        HBox hb1 = new HBox();

        //start/stop button
        Button a = new Button("Start/Stop");
        a.SetSizeRequest(350, 35);
        a.Clicked += Clicked;
        hb1.PackStart(a, false, false, 0);

        //clear button
        Button b = new Button("Clear");
        b.SetSizeRequest(350, 35);
        b.Clicked += Clear;
        hb1.PackStart(b, false, false, 0);

        vbox.PackStart(hb1, false, false, 0);



        HBox hb = new HBox();

        //speed button
        Label speedLr = new Label("Speed");
        speedLr.SetSizeRequest(100, 35);
        HScale speed = new HScale(0, 5, 1);
        speed.SetSizeRequest(200, 35);
        speed.ValueChanged += SpeedButton;

        //scale button
        Label scaleLr = new Label("Scale");
        scaleLr.SetSizeRequest(100, 35);
        HScale scale = new HScale(0, 2, 1);
        scale.SetSizeRequest(200, 35);
        scale.ValueChanged += ScaleButton;

        hb.PackStart(speedLr, false, false, 0);
        hb.PackStart(speed, false, false, 0);
        hb.PackStart(scaleLr, false, false, 0);
        hb.PackStart(scale, false, false, 0);

        vbox.PackStart(hb, false, false, 0);

        area = new DrawingArea();
        area.ExposeEvent += exposeHandler;
        vbox.PackStart(area, true, true, 0);

        this.Add(vbox);

    }

    void Clicked(object sender, EventArgs e)
    {
        this.simulation = !this.simulation;
        GLib.Timeout.Add(generationSpeed, new GLib.TimeoutHandler(sim));
    }

    void Clear(object sender, EventArgs e)
    {
        this.game.Clear();
        if (this.simulation)
            this.simulation = !this.simulation;
        QueueDraw();
    }

    void SpeedButton(object sender, EventArgs e)
    {
        HScale b = (HScale)sender;
        double val = b.Value;

        switch (val)
        {
            case 0:
                generationSpeed = 500;
                break;
            case 1:
                generationSpeed = 400;
                break;
            case 2:
                generationSpeed = 300;
                break;
            case 3:
                generationSpeed = 200;
                break;
            case 4:
                generationSpeed = 100;
                break;
            case 5:
                generationSpeed = 50;
                break;
        }
        if (this.simulation)
        {
            this.simulation = !this.simulation;
        }
        QueueDraw();
    }

    void ScaleButton(object sender, EventArgs e)
    {
        HScale b = (HScale)sender;
        double val = b.Value;

        switch (val)
        {
            case 0:
                scale = 1;
                break;
            case 1:
                scale = 2;
                break;
            case 2:
                scale = 5;
                break;
        }
        QueueDraw();
    }

    bool sim()
    {
        if (this.simulation)
        {
            this.game.NewGeneration();
            QueueDraw();
            return true;
        }
        else { return false; }
    }

    static void fillRectangle(Context c, int x, int y, int width, int height,
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

    protected override bool OnButtonPressEvent(EventButton e)
    {
        int x = (int)e.X;
        int y = (int)e.Y;
        Console.WriteLine($"{x / (10 * scale)} {(y - 70) / (10 * scale)}");
        if (this.game.gameArea[x / (10 * scale),
            (y - 70) / (10 * scale)] == 0)
        {
            this.game.gameArea[x / (10 * scale),
            (y-70) / (10 * scale)] = 1;
        }
        else
        {
            this.game.gameArea[x / (10 * scale),
            (y-70)/ (10 * scale)] = 0;
        }

        QueueDraw();
        return true;
    }

    void exposeHandler(object o, ExposeEventArgs args)
    {
        using (Context c = CairoHelper.Create(area.GdkWindow))
        {
            for (int i = 0; i*scale < this.game.column; i++)
            {
                for (int j = 0; j*scale < this.game.rows; j++)
                {
                    fillRectangle(c, i*10*scale, j*10*scale,
                        10*scale, 10*scale,this.game.gameArea[i, j]);
                }
            }
        }
    }

    protected override bool OnDeleteEvent(Event ev)
    {
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