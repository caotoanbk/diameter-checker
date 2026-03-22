path = r'D:\Projects\diameter-checker\Diameter_Checker\Diameter_Checker.csproj'
with open(path, 'r', encoding='utf-8') as f:
    content = f.read()

old = '    <None Include="App.config" />'
new = ('    <None Include="App.config" />\r\n'
       '    <Content Include="config.ini">\r\n'
       '      <CopyToOutputDirectory>Always</CopyToOutputDirectory>\r\n'
       '    </Content>')

if old in content:
    content = content.replace(old, new, 1)
    with open(path, 'w', encoding='utf-8') as f:
        f.write(content)
    print('Done - config.ini added to csproj with CopyToOutputDirectory=Always')
else:
    print('NOT FOUND - check the csproj manually')
