import { NbMenuItem } from '@nebular/theme';
import { Permission, Workscreen } from '../../api/models';

export interface TrixxMenuItem extends NbMenuItem {
  permission?: Permission;
  children?: TrixxMenuItem[];
}

// Если меняешь текст, не забудь поменять на бэке.
export const MENU_ITEMS: () => TrixxMenuItem[] = () => [
  {
    title: Workscreen.MainPage,
    icon: 'monitor-outline',
    link: 'main',
    home: true,
    permission: Permission.MainPage_Read,
  },
  {
    title: Workscreen.DictionaryCartoons,
    icon: 'film-outline',
    link: 'dictionary-cartoons',
    home: true,
    permission: Permission.DictionaryCartoons_Read,
  },
  {
    title: Workscreen.DictionaryStudios,
    icon: 'cube-outline',
    link: 'dictionary-studios',
    home: true,
    permission: Permission.DictionaryStudios_Read,
  },
  {
    title: Workscreen.TrixxUsers,
    icon: 'people-outline',
    link: 'users',
    home: true,
    permission: Permission.TrixxUsers_Read,
  },
  {
    title: Workscreen.TrixxRoles,
    icon: 'color-palette-outline',
    link: 'roles',
    home: true,
    permission: Permission.TrixxRoles_Read,
  },
  {
    title: 'Паки',
    icon: 'shopping-bag-outline',
    link: 'cartoons-packs',
    home: true,
    permission: Permission.CartoonsPack_Read,
  },
];
