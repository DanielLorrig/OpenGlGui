#version 400

out vec4 FragColor;
//in vec4 gl_FragCoord;
uniform vec2 iResolution;
uniform vec2 transpose;
uniform vec2 size;
uniform float isClicked;


uniform sampler2D texture0;

void main()
{
    //vec2 iResolution = vec2(1600, 900);
    vec2 p = gl_FragCoord.xy - transpose - iResolution / 2.0;
    vec2 q = size / 2.0;

    vec2 texCoords = vec2(p.x / size.x, -p.y / size.y) - 0.5;
    vec4 texture = texture(texture0, texCoords);
    FragColor = texture;
}