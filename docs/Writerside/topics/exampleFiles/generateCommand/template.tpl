# Test

Namespace: {{ Namespace }}
Enums Count: {{ Enums.size }}

{% for enum in Enums %}
- Enum: {{ enum.Name }}
  Desc: {{ enum.Desc }}
  {% for entry in enum.Entries %}
    * {{ entry.Name }} = {{ entry.Value }} - {{ entry.Desc }}
  {% endfor %}
{% endfor %}