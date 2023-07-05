using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace BricksGame
{
    /// <summary>
    ///  Contient l'UI du Scene Editor : les dés à placer, les flèches, le boutton save etc.
    /// </summary>
    public class EditorUI
    {
        private SceneEditor sceneEditor;
        public List<Dice> listOfDices;
        private Button previousDice;
        private Button nextDice;
        private float buttonsMargin = 40f;
        private Button bttn_Save;

        public EditorUI(SceneEditor p_sceneEditor) 
        { 
            sceneEditor = p_sceneEditor;
            LoadSaveButton();
            CreateSwitchDicesButtons();
            CreateDices();
        }

        private void LoadSaveButton()
        {
            List<Texture2D> myButtonTextureList = new List<Texture2D>() { ServiceLocator.GetService<ContentManager>().Load<Texture2D>("button_save"), ServiceLocator.GetService<ContentManager>().Load<Texture2D>("button_save_hover") };
            bttn_Save = new Button(myButtonTextureList);
            bttn_Save.onClick = Save;
            bttn_Save.onHover = OnHover;
            bttn_Save.Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Bounds.Width / 2 - bttn_Save.currentTexture.Width / 2, 50);
            sceneEditor.AddToGameObjectsList(bttn_Save);
        }

        //Methode appelant toutes les méthodes nécessaires pour créer les dés de selection
        private void CreateDices()
        {
            CreateDicesList();
            AddDices();
            SetToDice(Gamesystem.dice.d3);
            sceneEditor.currentNb = 3;
        }

        // Methode pour créer les boutons suivant/précédent auprès des dés
        private void CreateSwitchDicesButtons()
        {
            List<Texture2D> myButtonTextureList = new List<Texture2D>();
            myButtonTextureList.Add(ServiceLocator.GetService<ContentManager>().Load<Texture2D>("arrow"));
            LoadPreviousDiceButton(myButtonTextureList);
            LoadNextDiceButton(myButtonTextureList);

        }

        // Crée une liste de chaque dé pour l'afficher dans les dés disponibles
        public void CreateDicesList()
        {
            listOfDices = new List<Dice>();
            Dice d3 = new Dice(Gamesystem.dice.d3, false);
            Dice d4 = new Dice(Gamesystem.dice.d4, false);
            Dice d6 = new Dice(Gamesystem.dice.d6, false);
            Dice d8 = new Dice(Gamesystem.dice.d8, false);
            Dice d10 = new Dice(Gamesystem.dice.d10, false);
            Dice d12 = new Dice(Gamesystem.dice.d12, false);
            Dice d20 = new Dice(Gamesystem.dice.d20, false);
            listOfDices.Add(d3);
            listOfDices.Add(d4);
            listOfDices.Add(d6);
            listOfDices.Add(d8);
            listOfDices.Add(d10);
            listOfDices.Add(d12);
            listOfDices.Add(d20);
        }

        // Ajoute tous les dés de la liste aux objets de la scène pour les draw
        public void AddDices()
        {
            Rectangle Screen = ServiceLocator.GetService<GraphicsDevice>().Viewport.Bounds;
            for (int i = 0; i < listOfDices.Count; i++)
            {
                listOfDices[i].Position = new Vector2(Screen.Width / 2, Screen.Height - listOfDices[i].BoundingBox.Height);
                sceneEditor.AddToGameObjectsList(listOfDices[i]);
            }
        }

        // Méthode de sélection d'un dés :  cache/affiche les dés au clic sur prev ou next et modifie la valeur currentDice du sceneEditor (valeur qui sera appliquée sur la grille)
        public void SetToDice(Gamesystem.dice d)
        {
            sceneEditor.currentDice = d;

            foreach (Dice dice in listOfDices)
            {

                if (dice.value == d)
                {
                    dice.SetVisible(true);
                }
                else
                {
                    dice.SetVisible(false);
                }
            }
        }

        // Methode next permettant de sélectionner le dé suivant
        public void NextDice(Button p_Button)
        {
            switch (sceneEditor.currentDice)
            {
                case Gamesystem.dice.d3:
                    SetToDice(Gamesystem.dice.d4);
                    sceneEditor.currentNb = 4;
                    break;
                case Gamesystem.dice.d4:
                    SetToDice(Gamesystem.dice.d6);
                    sceneEditor.currentNb = 6;
                    break;
                case Gamesystem.dice.d6:
                    SetToDice(Gamesystem.dice.d8);
                    sceneEditor.currentNb = 8;
                    break;
                case Gamesystem.dice.d8:
                    SetToDice(Gamesystem.dice.d10);
                    sceneEditor.currentNb = 10;
                    break;
                case Gamesystem.dice.d10:
                    SetToDice(Gamesystem.dice.d12);
                    sceneEditor.currentNb = 12;
                    break;
                case Gamesystem.dice.d12:
                    SetToDice(Gamesystem.dice.d20);
                    sceneEditor.currentNb = 20;
                    break;
                case Gamesystem.dice.d20:
                    SetToDice(Gamesystem.dice.d3);
                    sceneEditor.currentNb = 3;
                    break;
                default:
                    SetToDice(Gamesystem.dice.d3);
                    sceneEditor.currentNb = 3;
                    break;
            }
        }

        // Methode prev permettant de sélectionner le dé précédent
        public void PreviousDice(Button p_Button)
        {

            switch (sceneEditor.currentDice)
            {
                case Gamesystem.dice.d3:
                    SetToDice(Gamesystem.dice.d20);
                    sceneEditor.currentNb = 20;
                    break;
                case Gamesystem.dice.d4:
                    SetToDice(Gamesystem.dice.d3);
                    sceneEditor.currentNb = 3;
                    break;
                case Gamesystem.dice.d6:
                    SetToDice(Gamesystem.dice.d4);
                    sceneEditor.currentNb = 4;
                    break;
                case Gamesystem.dice.d8:
                    SetToDice(Gamesystem.dice.d6);
                    sceneEditor.currentNb = 6;
                    break;
                case Gamesystem.dice.d10:
                    SetToDice(Gamesystem.dice.d8);
                    sceneEditor.currentNb = 8;
                    break;
                case Gamesystem.dice.d12:
                    SetToDice(Gamesystem.dice.d10);
                    sceneEditor.currentNb = 10;
                    break;
                case Gamesystem.dice.d20:
                    SetToDice(Gamesystem.dice.d12);
                    sceneEditor.currentNb = 12;
                    break;
                default:
                    SetToDice(Gamesystem.dice.d3);
                    sceneEditor.currentNb = 3;
                    break;
            }

        }

   
        private void LoadPreviousDiceButton(List<Texture2D> myButtonTextureList)
        {
            previousDice = new Button(myButtonTextureList);
            previousDice.Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Bounds.Width / 2 - previousDice.currentTexture.Width - buttonsMargin, ServiceLocator.GetService<GraphicsDevice>().Viewport.Bounds.Height - previousDice.currentTexture.Height * 2);
            previousDice.onClick = PreviousDice;
            previousDice.onHover = OnHover;
            sceneEditor.AddToGameObjectsList(previousDice);

        }

        private void LoadNextDiceButton(List<Texture2D> myButtonTextureList)
        {
            nextDice = new Button(myButtonTextureList);
            nextDice.Position = new Vector2(ServiceLocator.GetService<GraphicsDevice>().Viewport.Bounds.Width / 2 + buttonsMargin, ServiceLocator.GetService<GraphicsDevice>().Viewport.Bounds.Height - previousDice.currentTexture.Height * 2);
            nextDice.onClick = NextDice;
            nextDice.onHover = OnHover;
            nextDice.ChangeSpriteEffects(SpriteEffects.FlipHorizontally);
            sceneEditor.AddToGameObjectsList(nextDice);
        }

        // Active l'enregistrement JSON via SceneEditor
        public void Save(Button p_Button)
        {
            sceneEditor.Save(p_Button);
        }

        public void OnHover(Button p_Button)
        {

        }

    }
}
