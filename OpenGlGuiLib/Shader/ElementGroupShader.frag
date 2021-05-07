#version 400

out vec4 FragColor;
//in vec4 gl_FragCoord;
uniform vec2 iResolution;
uniform vec2 transpose;
uniform vec2 size;
uniform float isClicked;

float sdRoundBox(in vec2 p, in vec2 b, in vec4 r)
{
    r.xy = (p.x > 0.0) ? r.xy : r.zw;
    r.x = (p.y > 0.0) ? r.x : r.y;

    vec2 q = abs(p) - b + r.x;
    return min(max(q.x, q.y), 0.0) + length(max(q, 0.0)) - r.x;
}

void main()
{
    //vec2 iResolution = vec2(1600, 900);
    vec2 p = gl_FragCoord.xy - transpose - iResolution / 2.0;
    vec2 q = size / 2.0;

    float d = sdRoundBox(p, q, vec4(2.0));

    float isInsideBorder = 1.0 - smoothstep(-3.0, 0.0, d);

    float isInside = step(0.0, -d);

    FragColor = vec4(vec3(0.3),isInside);

    //vec3 border = vec3(0.0, 0.25, 0.25);
    //vec3 inside = vec3(0.0, 0.25, 0.4) * isInsideBorder;
    //vec3 col = border + inside;

    //col = col - isClicked * col * 0.45;

    //FragColor = objAndTextCol;
}