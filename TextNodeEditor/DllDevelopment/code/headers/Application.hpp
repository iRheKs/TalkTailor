#pragma once
#include "Dialogue.hpp"
/*!
 * Class that represents the application program of the DLL
 * 
 */
class Application 
{
private:
	/*!
	 * Instance to itself for the Singleton pattern
	 * 
	 */
	static Application* instance;
	/*!
	 * Private constructor for Singleton pattern
	 * 
	 */
	Application(){}

public:
	/*!
	 * Pointer to the current dialogue to work with
	 * 
	 */
	static Dialogue* currentDialogue;
	/*!
	 * Returns the instance of the current pointer to this class
	 * 
	 * \return pointer to the instance variable
	 */
	static Application* get_instance() 
	{
		if (instance == nullptr)
		{
			instance = new Application();
		}

		return instance;
	}

};