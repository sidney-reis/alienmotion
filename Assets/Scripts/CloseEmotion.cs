﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseEmotion : MonoBehaviour {
	Transform endgameMessage;
  Transform chestMessage;
  Transform resultMessage;
  Transform wrongMessage;

	public void Start() {
		endgameMessage = GameObject.Find("EndgameCanvas").transform.Find("EndgameMessage");
    chestMessage = GameObject.Find("MinigameCanvas").transform.Find("Image/Scroll View/Viewport/Content/chestMessage");
    wrongMessage = GameObject.Find("MinigameCanvas").transform.Find("Image/wrongMessage");
    resultMessage = GameObject.Find("MinigameCanvas").transform.Find("Image/resultMessage");
  }

  public void showMiniGameExplanation()
  {
    chestMessage.gameObject.SetActive(false);
    PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.SetUIExplanationText(PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].name);
    PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.sceneElement.gameObject.SetActive(true);
    PlayerInfo.current_step_game = PlayerInfo.STEP_LEARNING_MINIGAME;
  }

  public void playMiniGame()
  {
    PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.HideUIExplanation();
    PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.SetupMiniGame();
    PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.ShowMiniGame();
    PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.SetShortExplanation(PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].name);
    PlayerInfo.current_step_game = PlayerInfo.STEP_PLAYING_MINIGAME;
  }

  public void selectEmotion()
  {
    PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.ClearImagesColors();
    int responseCode = PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.ValidateAnswear().code;
    string responseMessage = PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.ValidateAnswear().message;

    resultMessage.gameObject.SetActive(true);
    if (responseMessage.Length > 0)
    {
      wrongMessage.gameObject.SetActive(true);
      wrongMessage.GetComponent<Text>().text = responseMessage;
    }
    else
    {
      wrongMessage.gameObject.SetActive(false);
    }

    if (responseCode == PlayerInfo.CORRECT_ANSWEAR) {
      resultMessage.GetComponent<Text>().text = "";
      bool hasNextChallenge = PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.HasNextChallenge();
      if (hasNextChallenge)
      {
        PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.NextChallenge();
      }
      else
      {
        chestMessage.gameObject.SetActive(true);
        PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.sceneElement.gameObject.SetActive(false);
        PlayerInfo.EMOTIONS[PlayerInfo.chestBeingPlayed].game.FinishGame();
        PlayerInfo.current_step_game = PlayerInfo.STEP_FINISHED_MINIGAME;
        ImageSelection.selectedImage0 = PlayerInfo.NOT_SELECTED_ANSWEAR;
        ImageSelection.selectedImage1 = PlayerInfo.NOT_SELECTED_ANSWEAR;
        ImageSelection.selectedImage2 = PlayerInfo.NOT_SELECTED_ANSWEAR;
        resultMessage.GetComponent<Text>().text = "";
        wrongMessage.GetComponent<Text>().text = "";
        resultMessage.gameObject.SetActive(false);
        wrongMessage.gameObject.SetActive(false);
        ok_click();
        return;
      }
    }
    else if(responseCode == PlayerInfo.NOT_SELECTED_ANSWEAR)
    {
      resultMessage.GetComponent<Text>().text = "";
    }
    else
    {
      resultMessage.GetComponent<Text>().text = "Que pena! Você errou!";
    }
    ImageSelection.selectedImage0 = PlayerInfo.NOT_SELECTED_ANSWEAR;
    ImageSelection.selectedImage1 = PlayerInfo.NOT_SELECTED_ANSWEAR;
    ImageSelection.selectedImage2 = PlayerInfo.NOT_SELECTED_ANSWEAR;
  }

  public void close()
  {
    Transform container = transform.Find("Image");
    container.gameObject.SetActive(false);
    PlayerInfo.chestBeingPlayed = -1;

		PlayerInfo.chestsFound++;
    GameObject.Find("HUD/Image/chestCounterText").GetComponent<Text>().text = PlayerInfo.chestsFound.ToString() + "/" + PlayerInfo.CHESTS_TO_WIN.ToString();

    PlayerInfo.current_step_game = PlayerInfo.STEP_NOT_PLAYING;

    if (PlayerInfo.chestsFound == PlayerInfo.CHESTS_TO_WIN)
    {
      endgameMessage.gameObject.SetActive(true);
      PlayerInfo.chestBeingPlayed = 99;
    }
  }

  public void ok_click()
  {
    switch (PlayerInfo.current_step_game)
    {
      case PlayerInfo.STEP_NOT_PLAYING:
        showMiniGameExplanation();
        break;
      case PlayerInfo.STEP_LEARNING_MINIGAME:
        playMiniGame();
        break;
      case PlayerInfo.STEP_PLAYING_MINIGAME:
        selectEmotion();
        break;
      case PlayerInfo.STEP_FINISHED_MINIGAME:
        close();
        break;
      default:
        close();
        break;
    }
  }
}
