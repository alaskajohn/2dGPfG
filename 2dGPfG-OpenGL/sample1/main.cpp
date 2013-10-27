#include <Windows.h>
#include <stdlib.h>
#include <stdio.h>
#include <iostream>

#include <glfw3.h>
// This project uses for interacting with the windowing system.  http://www.glfw.org/

#include <SOIL.h>
// This project uses the Simple OpenGL Image Library (SOIL) for loading PNG files.  http://www.lonesock.net/soil.html

using namespace std;


static void error_callback(int error, const char* description)
{
	cout << description << stderr;
}

static void key_callback(GLFWwindow* window, int key, int scancode, int action, int mods)
{
	if (key == GLFW_KEY_ESCAPE && action == GLFW_PRESS)
		glfwSetWindowShouldClose(window, GL_TRUE);
}

class cSprite
{
	GLuint m_iTexture;
	int m_iX, m_iY, m_iWidth, m_iHeight;
	
public:

	cSprite(GLuint texture_id, int x, int y, int width, int height)
	{
		m_iTexture = texture_id;
		m_iX = x;
		m_iY = y;
		m_iWidth = width;
		m_iHeight = height;
	}

	void draw(float dest_X, float dest_Y)
	{
		GLfloat drawColor[] = {1.0f, 1.0f, 1.0f};
		
		if (m_iTexture <= 0) return;
				
		glBegin(GL_QUADS);

			glBindTexture(GL_TEXTURE_2D, m_iTexture);

			glColor3fv(drawColor);
			glVertex3f(dest_X, dest_Y, 0.f);
			glTexCoord2f(0.0f, 1.0f);

			glColor3fv(drawColor);
			glVertex3f(dest_X, dest_Y + m_iHeight, 0.f);
			glTexCoord2f(1.0f, 1.0f);

			glColor3fv(drawColor);
			glVertex3f(dest_X + m_iWidth, dest_Y + m_iHeight, 0.f);
			glTexCoord2f(1.0f, 0.0f);

			glColor3fv(drawColor);
			glVertex3f(dest_X + m_iWidth, dest_Y, 0.f);
			glTexCoord2f(0.0f, 0.0f);

		glEnd();
	}
	
};

void createOrthoProjection(float ratio)
{
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	glOrtho(-0.5, 0.5, -0.5f, 0.5f, 0.5f, -0.5f);	//1x1x1 box centered at origin
}

void translateToXNACoords(int scrWidth, int scrHeight)
{
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	glTranslatef(-0.5f, 0.5f, 0.0f);											//set 0,0 as top left
	glScalef(1.0f/(float)scrWidth, -1.0f/(float)scrHeight, 0.f);				//flip y coords and scale to pixel size
}

int main(void)
{
	GLFWwindow* window;
	
	glfwSetErrorCallback(error_callback);

	if (!glfwInit())
		exit(EXIT_FAILURE);
		window = glfwCreateWindow(1280, 720, "2D Graphics Programming for Games", NULL, NULL);
	
		if (!window)
	{
		glfwTerminate();
		exit(EXIT_FAILURE);
	}

	glfwMakeContextCurrent(window);
	glfwSetKeyCallback(window, key_callback);

	//setup
	glEnable(GL_TEXTURE_2D);
	glEnable (GL_BLEND); 
	glBlendFunc (GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	GLuint tex_2d = SOIL_load_OGL_texture
	(
		"content/snow_assets.png",
		SOIL_LOAD_AUTO,
		SOIL_CREATE_NEW_ID,
		SOIL_FLAG_POWER_OF_TWO | SOIL_FLAG_COMPRESS_TO_DXT | SOIL_FLAG_MULTIPLY_ALPHA 
	);

	cSprite snowman = cSprite(tex_2d, 0, 0, 512, 512);

	if(tex_2d == 0)
	{
		cout << SOIL_last_result();
        return false;
	}
	
	glBindTexture(GL_TEXTURE_2D, tex_2d);
    glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR);
    glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_LINEAR);

	int width, height;
	glClearColor(0.4f, 0.6f, 0.95f, 1.0f); //Cornflower Blue
	
	while (!glfwWindowShouldClose(window))
	{
		glfwGetFramebufferSize(window, &width, &height);

		glViewport(0, 0, width, height);
		glClear(GL_COLOR_BUFFER_BIT);

		createOrthoProjection(width / (float) height);
		translateToXNACoords(width, height);
		
		//glRotatef((float) glfwGetTime() * 50.f, 0.f, 0.f, 1.f);
		snowman.draw(0,0);
		
		glfwSwapBuffers(window);
		glfwPollEvents();
	}

	glfwDestroyWindow(window);
	glfwTerminate();
	
	exit(EXIT_SUCCESS);
}

