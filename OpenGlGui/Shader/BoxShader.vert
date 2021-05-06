#version 400 core

layout(location = 0) in vec3 aPosition;

uniform vec2 iResolution;
uniform vec2 transpose;
uniform vec2 size;

void main()
{
	gl_Position = vec4(aPosition.xy * size / iResolution + 2.0 * transpose / iResolution, 0.0, 1.0);
}