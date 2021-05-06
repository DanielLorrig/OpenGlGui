#version 400 core

layout(location = 0) in vec3 aPosition;

uniform vec2 iResolution;
uniform vec2 transpose;

void main()
{
	//gl_Position = transform * vec4(aPosition, 1.0);
	//gl_Position = vec4((aPosition.xy+transpose)/ iResolution, 0, 1.0);
	gl_Position = vec4(((aPosition.xy + transpose) / (iResolution.xy / 2.0)), 0.0, 1.0);
}