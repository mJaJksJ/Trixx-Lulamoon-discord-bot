import { NbMenuItem } from '@nebular/theme';
import { Permission } from '../../api/models';

export interface TrixxMenuItem extends NbMenuItem {
  permission?: Permission;
  children?: TrixxMenuItem[];
}

// Если меняешь текст, не забудь поменять на бэке.
export const MENU_ITEMS: () => TrixxMenuItem[] = () => [
  {
    title: 'Главная',
    icon: 'monitor-outline',
    link: '/pages/main',
    home: true,
    permission: Permission.MainPageRead,
  },
];
