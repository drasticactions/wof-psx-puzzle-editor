using System;
using Gtk;
using GLib;
using System.Collections.Generic;
using System.IO;
using WheelFun.Shared;
using System.Linq;
using System.Text;

public partial class MainWindow : Gtk.Window
{
    TreeStore questionListStore;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();

        Gtk.TreeViewColumn questionColumn = new Gtk.TreeViewColumn ();
        questionColumn.Title = "Questions";
        Gtk.CellRendererText quesitonNameCell = new Gtk.CellRendererText ();
        quesitonNameCell.Editable = true;
        quesitonNameCell.Edited += questionNameCell_Edited;
        questionColumn.PackStart (quesitonNameCell, true);
        questionColumn.AddAttribute (quesitonNameCell, "text", 0);
        treeview1.AppendColumn (questionColumn);
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void OnExit(object sender, EventArgs e)
    {
        Application.Quit ();
    }

    protected void OnOpen(object sender, EventArgs e)
    {
        FileChooserDialog fileChooser = new FileChooserDialog ("Load PUZZLES.BIN", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
        if (fileChooser.Run () == (int)ResponseType.Accept) {

            if(fileChooser.Filename.ToLower().EndsWith("puzzles.bin"))
            {
                GenerateTable(fileChooser.Filename);
            }
        }

        fileChooser.Destroy ();
    }

    protected void OnAbout(object sender, EventArgs e)
    {
        var about = new AboutDialog ();
        about.ProgramName = "Wheel Of Fortune (PSX) Puzzle Editor";
        about.Authors = new List<string>() {"DrasticActions"}.ToArray();
        about.Version = "0.0.1";
        about.Run ();

        about.Destroy ();
    }

    private void questionNameCell_Edited (object o, Gtk.EditedArgs args)
    {
        if (args.NewText.Length <= 0 || args.NewText.Length > 52) {
            return;
        }
        var replacementText = args.NewText.ToUpper();
        Gtk.TreeIter iter;
        questionListStore.GetIter (out iter, new Gtk.TreePath (args.Path));
        questionListStore.SetValue(iter, 0, replacementText);
    }

    private void GenerateTable(string filepath) {
        
        questionListStore = new Gtk.TreeStore (typeof (string));

        var puzzleBytes = File.ReadAllBytes(filepath);
        var gameList = Helpers.GetDefaultGameLists();
        foreach(var category in gameList) {
            if (category.GameType == GameType.Clue) {
                // We're skipping "Clue" games for now.
                continue;
            }
            Gtk.TreeIter iter = questionListStore.AppendValues (category.Category);
            var startIndex = category.Start;
            while (true) {
                startIndex = Helpers.FindIndex(puzzleBytes, category.StartByte, startIndex);
                if (startIndex >= category.End) {
                    // We're out of the byte range for this category, break.
                    break;
                }
                // Each question is 52 bytes, so we're taking the question itself,
                // And leaving the control bytes alone.
                var endIndex = startIndex + 54;
                var destArray = puzzleBytes.Skip(startIndex + 2).Take((endIndex - startIndex) - 2).ToArray();
                var test = Helpers.ConvertToString(destArray);
                questionListStore.AppendValues (iter, test);
                // Start the next loop with the next control byte.
                startIndex = startIndex + 1;
            }
        }
        treeview1.Model = questionListStore;
    }
}
