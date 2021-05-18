#version 330 core
struct Material{    
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};

struct Light{
    vec3 direction;
    vec3 position;
    float cutOff;
    float outerCutOff;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float constant;
    float linear;
    float quadratic;
};

uniform Light light;

out vec4 FragColor;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoords;


uniform vec3 lightPos;
uniform vec3 viewPos;
uniform Material material;

void main()
{




    float distance = length(light.position - FragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
    light.quadratic * (distance * distance));

    // ambient
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));

    
    // diffuse
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    //vec3 lightDir = normalize(-light.direction);
    float diff = max(dot(norm, lightDir), 0);
    vec3 diffuse = light.diffuse * (diff * vec3(texture(material.diffuse, TexCoords)));

    // specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0) , material.shininess);
    vec3 specular = light.specular * (vec3(texture(material.specular, TexCoords)) * spec);
    

    //FragColor = texture(material.diffuse, TexCoords);
    //FragColor = texture(ourTexture, TexCoords);


    float theta = dot(lightDir, normalize(-light.direction));
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    
    vec3 result = ambient * attenuation + diffuse * attenuation * intensity + specular * attenuation * intensity;
    FragColor = vec4(result, 1.0);
    
}