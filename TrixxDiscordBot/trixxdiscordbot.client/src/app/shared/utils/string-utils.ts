export const comareStrings = (str: string, compareStrs: string[]) => {
    const simplyStr = simplifyStr(str);
    for (const compareStr of compareStrs) {
        const simplyCompareStr = simplifyStr(compareStr)
        if (!simplyStr.includes(simplyCompareStr) && !simplyCompareStr.includes(simplyStr)) {
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