using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    /*public Blocks[] boxes; // Array to hold references to all blocks
    public string targetWord = "WRATH"; // The word that needs to be spelled
    public Door canOpen;
    public Snapping[] surfaces;

    void Update()
    {
        if (AreAllBoxesSnapped() && IsWordSpelledCorrectly())
        {
            canOpen.openClose();
        }
    }

    bool AreAllBoxesSnapped()
    {
        foreach (Snapping surface in surfaces)
        {
            if (!surface.isSnapped)
            {
                return false; // At least one box is not snapped
            }
        }
        return true; // All boxes are snapped
    }

    bool IsWordSpelledCorrectly()
    {
        string spelledWord = "";

        foreach (Blocks box in boxes)
        {
            // Check if the face is correctly oriented
            if (!box.IsFaceCorrectlyOriented())
            {
                return false; // The face is not correctly oriented
            }

            // Get the letter on the currently visible face
            char visibleLetter = box.lettersOnFaces[box.currentVisibleFaceIndex];
            spelledWord += visibleLetter;
        }

        return spelledWord == targetWord;
    }
    
    public string targetWord = "WORD";
    public float maxDistanceBetweenBlocks = 1.5f;
    public GameObject door;

    private List<Blocks> allBlocks;
    public Door canOpen;
    public Snapping[] surfaces;

    private void Start()
    {
        allBlocks = FindObjectsOfType<Blocks>().ToList();
    }

    private void Update()
    {
        if (AreAllBoxesSnapped() && CheckWord())
        {
            canOpen.openClose();
        }
    }
    
    bool AreAllBoxesSnapped()
    {
        foreach (Snapping surface in surfaces)
        {
            if (!surface.isSnapped)
            {
                return false; // At least one box is not snapped
            }
        }
        return true; // All boxes are snapped
    }

    private bool CheckWord()
    {
        for (int i = 0; i < allBlocks.Count; i++)
        {
            List<Blocks> wordBlocks = new List<Blocks> { allBlocks[i] };
            if (TryFormWord(wordBlocks, 1))
            {
                return true;
            }
        }
        return false;
    }

    private bool TryFormWord(List<Blocks> currentBlocks, int nextLetterIndex)
    {
        if (nextLetterIndex == targetWord.Length)
        {
            return IsValidWord(currentBlocks);
        }

        Blocks lastBlock = currentBlocks[currentBlocks.Count - 1];
        foreach (Blocks block in allBlocks)
        {
            if (!currentBlocks.Contains(block) && 
                Vector3.Distance(lastBlock.GetPosition(), block.GetPosition()) <= maxDistanceBetweenBlocks)
            {
                currentBlocks.Add(block);
                if (TryFormWord(currentBlocks, nextLetterIndex + 1))
                {
                    return true;
                }
                currentBlocks.RemoveAt(currentBlocks.Count - 1);
            }
        }
        return false;
    }

    private bool IsValidWord(List<Blocks> blocks)
    {
        string formedWord = "";
        foreach (Blocks block in blocks)
        {
            if (block.IsLetterUpsideDown())
            {
                return false; // If any letter is upside down, the word is invalid
            }
            formedWord += block.GetVisibleLetter();
        }
        return formedWord == targetWord;
    }*/
    
    public Door canOpen;
    public Snapping[] surfaces;
    private bool hasOpened = false;

    void Update()
    {
        if (AreAllBoxesSnapped() && !hasOpened)
        {
            canOpen.openClose();
            hasOpened = true;
        }
    }

    bool AreAllBoxesSnapped()
    {
        foreach (Snapping surface in surfaces)
        {
            if (!surface.isSnapped)
            {
                return false; // At least one box is not snapped
            }
        }
        return true; // All boxes are snapped
    }
}
