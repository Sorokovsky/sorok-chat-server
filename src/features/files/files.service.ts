import { Injectable } from "@nestjs/common";
import { join } from "node:path";
import { SERVER_FOLDER } from "@constants/default.constant";
import { lstat, mkdir, rm, writeFile } from "node:fs/promises";
import { Stats } from "node:fs";
import { PathNotFoundException } from "@exceptions/path/path-not-found.exception";
import { PathAlreadyExistsException } from "@exceptions/path/path-already-exists.exception";

@Injectable()
export class FilesService {
  public async upload(
    file: Express.Multer.File,
    folder: string,
    name?: string,
    rewrite: boolean = false,
  ): Promise<string> {
    const extension: string = this.getExtension(file.originalname);
    const fileName: string = name ?? this.getFileName(file.originalname);
    const newFileName: string = `${fileName}.${extension}`;
    const resultPath: string = join(folder, newFileName);
    const serverFolder: string = join(SERVER_FOLDER, folder);
    const serverPath: string = join(serverFolder, newFileName);
    const isPathExists: boolean = await this.isPathExists(serverPath);
    if (isPathExists === true && rewrite === false) {
      throw new PathAlreadyExistsException(resultPath);
    }
    if ((await this.isFolderExist(serverFolder)) === false) {
      await mkdir(serverFolder, { recursive: true });
    }
    await writeFile(serverPath, file.buffer);
    return resultPath;
  }

  public async delete(path: string, withError: boolean = true): Promise<void> {
    const serverPath: string = join(SERVER_FOLDER, path);
    const isPathExists: boolean = await this.isPathExists(serverPath);
    if (isPathExists === false && withError) {
      throw new PathNotFoundException(path);
    }
    if (isPathExists) {
      return await rm(serverPath, { force: true, recursive: true });
    }
  }

  private getExtension(fileFullName: string): string {
    const parts: string[] = fileFullName.split(".");
    return parts.at(-1);
  }

  private getFileName(fileFullName: string): string {
    const parts: string[] = fileFullName.split(".");
    return parts[0];
  }

  private async isFolderExist(folder: string): Promise<boolean> {
    try {
      const stats: Stats = await lstat(folder);
      return stats.isDirectory();
    } catch {
      return false;
    }
  }

  private async isPathExists(path: string): Promise<boolean> {
    try {
      await lstat(path);
      return true;
    } catch {
      return false;
    }
  }
}
