export const comareStrings = (str: string, compareStrs: string[]) => {
    const simplyStr = simplifyStr(str);
    for (const compareStr of compareStrs) {
        if (simplyStr !== simplifyStr(compareStr)) {
            return false;
        }
    }
    return true;
}

export const simplifyStr = (str: string) => {
    return str
        .replace(/\s+/g, '')
        .toLowerCase()
        .replace(/ё/g, 'е');
}